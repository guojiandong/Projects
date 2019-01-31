﻿using DotnetSpider.Core.Monitor;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Scheduler;
using DotnetSpider.Downloader;
using DotnetSpider.Downloader.Redial;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using DotnetSpider.Core.Infrastructure;
#if NETFRAMEWORK
using System.Net;
#else
using System.Text;

#endif

[assembly: InternalsVisibleTo("DotnetSpider.Core.Test")]
[assembly: InternalsVisibleTo("DotnetSpider.Sample")]
[assembly: InternalsVisibleTo("DotnetSpider.Extension")]
[assembly: InternalsVisibleTo("DotnetSpider.Extension.Test")]

namespace DotnetSpider.Core
{
	/// <summary>
	/// A spider contains four modules: Downloader, Scheduler, PageProcessor and Pipeline. 
	/// </summary>
	public class Spider : AppBase, ISpider, ISpeedMonitor
	{
		private IScheduler _scheduler = new QueueDuplicateRemovedScheduler();
		private IDownloader _downloader = new HttpClientDownloader();
		private List<ResultItems> _cached;
		private int _waitCountLimit = 1500;
		private bool _inited;
		private int _threadNum = 1;
		private bool _skipTargetRequestsWhenResultIsEmpty = true;
		private bool _exitWhenComplete = true;
		private int _emptySleepTime = 15000;
		private int _pipelineCachedSize = 1;
		private RetryPolicy _pipelineRetry;
		private readonly AutomicLong _requestedCount = new AutomicLong(0);
		private readonly MemoryMappedFile[] _mmfCloseSignals = new MemoryMappedFile[2];
		private readonly string[] _fileCloseSignals = new string[2];
		private readonly List<Request> _requests = new List<Request>();
		private bool _exited;
		private IMonitor _monitor;
		private int _statusFlushInterval = 5000;
		private int _monitorFlushInterval;
		private Serilog.ILogger _failingRequestsLogger;
		private long _downloaderTimes;
		private long _pipelineTimes;
		private long _processorTimes;
		private long _downloaderCostTimes;
		private long _pipelineCostTimes;
		private long _processorCostTimes;
		private int _depth = int.MaxValue;

		private readonly Dictionary<string, Dictionary<string, object>> _headers =
			new Dictionary<string, Dictionary<string, object>>();

		/// <summary>
		/// 自定义的初始化
		/// </summary>
		/// <param name="arguments">运行参数</param>
		protected virtual void OnInit(params string[] arguments)
		{
		}

		/// <summary>
		/// 是否需要通过StartUrlsBuilder来初始化起始链接
		/// </summary>
		/// <param name="arguments">程序运行参数</param>
		/// <returns>返回 True, 则需要执行所有注册的StartUrlsBulder.</returns>
		protected virtual bool IfRequireRequestBuilders(string[] arguments)
		{
			return arguments.Any(t => t?.ToLower() == SpiderArguments.ExcludeRequestBuilder);
		}

		protected virtual bool IfRequireReport(string[] arguments)
		{
			return arguments.Any(t => t?.ToLower() == SpiderArguments.Report);
		}

		/// <summary>
		/// 通过StartUrlsBuilder来初始化起始链接后的响应操作
		/// </summary>
		protected virtual void OnRequestBuildersCompleted()
		{
		}

		/// <summary>
		/// 数据验证和生成报告
		/// </summary>
		protected virtual void OnReporting()
		{
		}

		protected virtual void OnReported()
		{
		}

		protected void Report(string[] arguments)
		{
			NetworkCenter.Current.Execute("report", () =>
			{
				if (!IfRequireReport(arguments)) return;
				try
				{
					Logger.LogInformation("Report start...");
					OnReporting();
					Logger.LogInformation("Report complete");
				}
				finally
				{
					OnReported();
				}
			});
		}

		/// <summary>
		/// All pipelines for spider.
		/// </summary>
		public readonly List<IPipeline> Pipelines = new List<IPipeline>();

		/// <summary>
		/// Storage all processors for spider.
		/// </summary>
		public readonly List<IPageProcessor> PageProcessors = new List<IPageProcessor>();

		/// <summary>
		/// Interval time wait for new url.
		/// </summary>
		protected int WaitInterval { get; } = 10;

		/// <summary>
		/// 遍历深度
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		public int Depth
		{
			get => _depth;
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException("Depth should be greater than 0.");
				}

				_depth = value;
			}
		}

		/// <summary>
		/// Whether spider is complete.
		/// </summary>
		public bool IsCompleted { get; private set; }

		/// <summary>
		/// Record how many times retried.
		/// </summary>
		public AutomicLong RetriedTimes { get; } = new AutomicLong();

		/// <summary>
		/// Status of spider.
		/// </summary>
		public Status Status { get; private set; } = Status.Init;

		/// <summary>
		/// 每处理完一个目标链接后停顿的时间, 单位毫秒 
		/// </summary>
		public int SleepTime { get; set; } = 100;

		/// <summary>
		/// 目标链接的最大重试次数
		/// </summary>
		public int CycleRetryTimes { get; set; } = 5;

		/// <summary>
		/// Event of crawler a request success.
		/// </summary>
		public event Action<Request> OnRequestSucceeded;

		/// <summary>
		/// Event of crawler on closed.
		/// </summary>
		public event Action<Spider, bool> OnClosed;

		public readonly List<IBeforeProcessorHandler> BeforeProcessors = new List<IBeforeProcessorHandler>();

		/// <summary>
		/// Whether clear scheduler after spider completed.
		/// 爬虫完成的定义是指队列中再也没有需要采集的请求, 而不是爬虫退出. 对于内存型爬虫来说, 这个值的设置没有关系, 因为程序关闭后内存自然释放掉了. 此值主要是用在分布式队列
		/// 中, 队列中的Request数量为0则表达整个爬虫结束, 如果此值为true, 则需要调用分布式队列的清空方法(分布式队列中会保存所以已经采集过的Request, 以及所有Request用于判断去重, 
		/// 对于量大的爬虫会导到分布式队列的存储爆炸, 所以需要清理)
		/// </summary>
		public bool ClearSchedulerAfterCompleted { get; set; } = true;

		/// <summary>
		/// Monitor of spider.
		/// </summary>
		public IMonitor Monitor
		{
			get => _monitor;
			set
			{
				if (_monitor == value) return;
				
				CheckIfRunning();

				_monitor = value;
				if (_monitor != null)
				{
					_monitor.Logger = Logger;
				}
			}
		}

		/// <summary>
		/// Average speed downloader.
		/// </summary>
		public long AvgDownloadSpeed { get; private set; }

		/// <summary>
		/// Average speed processor.
		/// </summary>
		public long AvgProcessorSpeed { get; private set; }

		/// <summary>
		/// Average speed pipeline.
		/// </summary>
		public long AvgPipelineSpeed { get; private set; }

		/// <summary>
		/// 上报运行状态的间隔
		/// </summary>
		public int StatusFlushInterval
		{
			get => _statusFlushInterval;
			set
			{
				CheckIfRunning();

				if (value < 2000)
				{
					throw new ArgumentException($"{nameof(StatusFlushInterval)} should greater than 2000.");
				}

				if (value > 60000)
				{
					throw new ArgumentException($"{nameof(StatusFlushInterval)} should less than 60000.");
				}

				_statusFlushInterval = value;
			}
		}

		/// <summary>
		/// Set the retry times for pipeline.
		/// </summary>
		public int PipelineRetryTimes { get; set; }

		/// <summary>
		/// Scheduler of spider.
		/// </summary>
		public IScheduler Scheduler
		{
			get => _scheduler;
			set
			{
				CheckIfRunning();
				_scheduler = value ?? throw new ArgumentNullException(nameof(Scheduler));
			}
		}

		/// <summary>
		/// The number of request pipeline handled every time.
		/// </summary>
		public int PipelineCachedSize
		{
			get => _pipelineCachedSize;
			set
			{
				CheckIfRunning();
				if (value <= 0)
				{
					throw new ArgumentException($"{nameof(PipelineCachedSize)} should be greater than 0.");
				}

				_pipelineCachedSize = value;
			}
		}

		/// <summary>
		/// Interface used to adsl redial.
		/// </summary>
		public IRedialExecutor RedialExecutor
		{
			get => NetworkCenter.Current.Executor;
			set
			{
				CheckIfRunning();
				NetworkCenter.Current.Executor = value;
			}
		}

		/// <summary>
		/// Downloader of spider.
		/// </summary>
		public IDownloader Downloader
		{
			get => _downloader;
			set
			{
				CheckIfRunning();
				_downloader = value ?? throw new ArgumentNullException(nameof(Downloader));
			}
		}

		/// <summary>
		/// Spider will exit if there is no any other request after waitting this time.
		/// </summary>
		public int EmptySleepTime
		{
			get => _emptySleepTime;
			set
			{
				CheckIfRunning();

				if (value > 0)
				{
					_emptySleepTime = value;
					_waitCountLimit = value / WaitInterval;
				}
				else if (value == 0)
				{
					_emptySleepTime = 0;
					_waitCountLimit = 0;
				}
				else
				{
					throw new ArgumentException($"{nameof(EmptySleepTime)} should be greater than 0.");
				}
			}
		}

		/// <summary>
		/// Whether exit spider after complete.
		/// </summary>
		public bool ExitWhenComplete
		{
			get => _exitWhenComplete;
			set
			{
				CheckIfRunning();
				_exitWhenComplete = value;
			}
		}

		/// <summary>
		/// 编码
		/// </summary>
		public string EncodingName { get; set; }

		/// <summary>
		/// Thread number of spider.
		/// </summary>
		public int ThreadNum
		{
			get => _threadNum;
			set
			{
				CheckIfRunning();

				if (value <= 0)
				{
					throw new ArgumentException($"{nameof(ThreadNum)} should be more than one!");
				}

				_threadNum = value;
			}
		}

		/// <summary>
		/// Whether skip request when results of processor.
		/// When results of processor is empty will retry request if this value is false.
		/// </summary>
		public bool SkipTargetRequestsWhenResultIsEmpty
		{
			get => _skipTargetRequestsWhenResultIsEmpty;
			set
			{
				CheckIfRunning();
				_skipTargetRequestsWhenResultIsEmpty = value;
			}
		}

		/// <summary>
		/// Monitor to get success count, error count, speed info etc.
		/// </summary>
		public IMonitorable Monitorable => Scheduler;

		public readonly List<IRequestBuilder> RequestBuilders = new List<IRequestBuilder>();

		/// <summary>
		/// 构造方法
		/// </summary>
		public Spider() : this(Guid.NewGuid().ToString("N"), new QueueDuplicateRemovedScheduler(), null,
			null)
		{
		}


		/// <summary>
		/// Create a spider with site, identity, scheduler and pageProcessors.
		/// </summary>
		/// <param name="identity">唯一标识</param>
		/// <param name="scheduler">调度队列</param>
		/// <param name="pageProcessors">页面解析器</param>
		/// <param name="pipelines">数据管道</param>
		public Spider(string identity, IScheduler scheduler, IEnumerable<IPageProcessor> pageProcessors,
			IEnumerable<IPipeline> pipelines)
		{
			ThreadPool.SetMinThreads(256, 256);
#if NETSTANDARD
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#else
			ServicePointManager.DefaultConnectionLimit = 1000;
#endif
			Identity = identity;
			Scheduler = scheduler;

			if (pageProcessors != null)
			{
				PageProcessors.AddRange(pageProcessors);
			}

			if (pipelines != null)
			{
				Pipelines.AddRange(pipelines);
			}
		}

		/// <summary>
		/// Create a spider with pageProcessors.
		/// 不需要指定标识, 使用内存队列, 使用默认HttpClient下载器, 允许没有Pipeline
		/// </summary>
		/// <param name="pageProcessors">页面解析器</param>
		/// <returns>爬虫</returns>
		public static Spider Create(params IPageProcessor[] pageProcessors)
		{
			return new Spider(Guid.NewGuid().ToString("N"), new QueueDuplicateRemovedScheduler(),
				pageProcessors, null);
		}

		/// <summary>
		/// Create a spider with pageProcessors and scheduler.
		/// </summary>
		/// <param name="pageProcessors">页面解析器</param>
		/// <param name="scheduler">调度队列</param>
		/// <returns>爬虫</returns>
		public static Spider Create(IScheduler scheduler, params IPageProcessor[] pageProcessors)
		{
			return new Spider(Guid.NewGuid().ToString("N"), scheduler, pageProcessors, null);
		}

		/// <summary>
		/// Create a spider with pageProcessors and scheduler.
		/// </summary>
		/// <param name="identify">唯一标识</param>
		/// <param name="pageProcessors">页面解析器</param>
		/// <param name="scheduler">调度队列</param>
		/// <returns>爬虫</returns>
		public static Spider Create(string identify, IScheduler scheduler,
			params IPageProcessor[] pageProcessors)
		{
			return new Spider(identify, scheduler, pageProcessors, null);
		}

		public Spider AddHeaders(string host, Dictionary<string, object> headers)
		{
			CheckIfRunning();
			var key = host.ToLower();
			if (_headers.ContainsKey(key))
			{
				foreach (var kv in headers)
				{
					_headers[key].Add(kv.Key, kv.Value);
				}
			}
			else
			{
				_headers.Add(key, headers);
			}

			return this;
		}

		/// <summary>
		/// Add IBeforeProcessorHandler
		/// </summary>
		/// <param name="beforeProcessorHandler"></param>
		/// <returns></returns>
		public Spider AddBeforeProcessor(IBeforeProcessorHandler beforeProcessorHandler)
		{
			BeforeProcessors.Add(beforeProcessorHandler);
			return this;
		}

		/// <summary>
		/// Add start url builder to spider.
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		public Spider AddRequestBuilder(IRequestBuilder builder)
		{
			if (builder == null)
			{
				throw new ArgumentNullException(nameof(builder));
			}

			CheckIfRunning();
			RequestBuilders.Add(builder);
			return this;
		}

		/// <summary>
		/// Add urls to spider.
		/// </summary>
		/// <param name="url">链接</param>
		/// <param name="extras">Extra properties of request.</param>
		/// <returns></returns>
		public Spider AddRequest(string url, Dictionary<string, dynamic> extras)
		{
			return AddRequests(new Request(url, extras));
		}

		/// <summary>
		/// Add urls to crawl.
		/// </summary>
		/// <param name="urls">链接</param>
		/// <returns>爬虫</returns>
		public Spider AddRequests(params string[] urls)
		{
			if (urls == null)
			{
				throw new ArgumentNullException(nameof(urls));
			}

			return AddRequests(urls.AsEnumerable());
		}

		/// <summary>
		/// Add startUrls to spider. 
		/// </summary>
		/// <param name="urls">链接</param>
		/// <returns>爬虫</returns>
		public Spider AddRequests(IEnumerable<string> urls)
		{
			if (urls == null)
			{
				throw new ArgumentNullException(nameof(urls));
			}

			return AddRequests(urls.Select(u => new Request(u, null)));
		}

		/// <summary>
		/// Add urls with information to crawl.
		/// </summary>
		/// <param name="request">Request</param>
		/// <returns>Spider</returns>
		public Spider AddRequest(Request request)
		{
			return AddRequests(request);
		}

		/// <summary>
		/// Add urls with information to crawl.
		/// </summary>
		/// <param name="requests">链接</param>
		/// <returns>爬虫</returns>
		public Spider AddRequests(params Request[] requests)
		{
			if (requests == null)
			{
				throw new ArgumentNullException(nameof(requests));
			}

			return AddRequests(requests.AsEnumerable());
		}

		/// <summary>
		/// Add urls with information to crawl.
		/// </summary>
		/// <param name="requests">链接</param>
		/// <returns>爬虫</returns>
		public Spider AddRequests(IEnumerable<Request> requests)
		{
			if (requests == null)
			{
				throw new ArgumentNullException(nameof(requests));
			}

			CheckIfRunning();
			foreach (var request in requests)
			{
				_requests.Add(request);
			}

			return this;
		}

		/// <summary>
		/// Add a page processor to spider.
		/// </summary>
		/// <param name="processor">页面解析器</param>
		/// <returns>爬虫</returns>
		public virtual Spider AddPageProcessor(IPageProcessor processor)
		{
			return AddPageProcessors(processor);
		}

		/// <summary>
		/// Add page processors to spider.
		/// </summary>
		/// <param name="processors">页面解析器</param>
		/// <returns>爬虫</returns>
		public virtual Spider AddPageProcessors(params IPageProcessor[] processors)
		{
			if (processors == null)
			{
				throw new ArgumentNullException(nameof(processors));
			}

			return AddPageProcessors(processors.AsEnumerable());
		}

		/// <summary>
		/// Add page processors to spider.
		/// </summary>
		/// <param name="processors">页面解析器</param>
		/// <returns>爬虫</returns>
		public virtual Spider AddPageProcessors(IEnumerable<IPageProcessor> processors)
		{
			if (processors != null)
			{
				CheckIfRunning();
				foreach (var processor in processors)
				{
					if (processor != null)
					{
						PageProcessors.Add(processor);
					}
				}
			}

			return this;
		}

		/// <summary>
		/// Add a pipeline for Spider
		/// </summary>
		/// <param name="pipeline">数据管道</param>
		/// <returns>爬虫</returns>
		public virtual Spider AddPipeline(IPipeline pipeline)
		{
			return AddPipelines(pipeline);
		}

		/// <summary>
		/// Set pipelines for Spider
		/// </summary>
		/// <param name="pipelines">数据管道</param>
		/// <returns>爬虫</returns>
		public virtual Spider AddPipelines(params IPipeline[] pipelines)
		{
			if (pipelines == null)
			{
				throw new ArgumentNullException(nameof(pipelines));
			}

			return AddPipelines(pipelines.AsEnumerable());
		}

		/// <summary>
		/// Set pipelines for Spider
		/// </summary>
		/// <param name="pipelines">数据管道</param>
		/// <returns>爬虫</returns>
		public virtual Spider AddPipelines(IEnumerable<IPipeline> pipelines)
		{
			if (pipelines == null)
			{
				throw new ArgumentNullException(nameof(pipelines));
			}

			CheckIfRunning();
			foreach (var pipeline in pipelines)
			{
				if (pipeline != null)
				{
					Pipelines.Add(pipeline);
				}
			}

			return this;
		}

		/// <summary>
		/// Run spider.
		/// </summary>
		/// <param name="arguments">运行参数</param>
		protected override void Execute(params string[] arguments)
		{
			if (_inited || Status == Status.Running)
			{
				Logger.LogWarning("Crawler is running...");
				return;
			}

			if (arguments.Any(t => t?.ToLower() == SpiderArguments.Report))
			{
				Report(arguments);
				return;
			}

			Logger.LogInformation("On init...");

			NetworkCenter.Current.Execute("onInit", () => { OnInit(arguments); });

			InitComponents(arguments);

			if (arguments.Any(a => a?.ToLower() == SpiderArguments.InitOnly))
			{
				return;
			}

			Status = Status.Running;
			_exited = false;

			FlushStatus();

			while (Status == Status.Running || Status == Status.Paused)
			{
				// 暂停则一直停在此处
				if (Status == Status.Paused)
				{
					Thread.Sleep(50);
					continue;
				}

				Parallel.For(0, ThreadNum, new ParallelOptions
				{
					MaxDegreeOfParallelism = ThreadNum
				}, i =>
				{
					int waitCount = 1;
					// 每个线程使用一个下载器实例, 在使用如WebDriverDownloader时不需要管理WebDriver实例了
					var downloader = Downloader.Clone();
					while (Status == Status.Running)
					{
						// 从队列中取出一个请求, 因为 Site 是共享对象， 每个Request都保留了引用， 序列存到Redis或其它数据库中浪费带宽和空间， 因此 Site对象不保存到数据库中
						Request request = Scheduler.Poll();
						// 如果队列中没有需要处理的请求, 则开始等待, 一直到设定的 EmptySleepTime 结束, 则认为爬虫应该结束了
						if (request == null)
						{
							if (waitCount > _waitCountLimit && ExitWhenComplete)
							{
								Status = Status.Finished;
								break;
							}

							// wait until new url added
							WaitNewUrl(ref waitCount);
						}
						else
						{
							waitCount = 1;

							try
							{
								Stopwatch sw = new Stopwatch();
								var result = HandleRequest(sw, request, downloader);
								if (result.Item1)
								{
									OnSuccess(request);
									Logger.LogInformation(
										$"Crawl {request.Url} success, results {result.Item2}, effectedRow {result.Item3}.");
								}
								else
								{
									OnError(request);
								}

								Thread.Sleep(SleepTime);
							}
							catch (ExitException)
							{
								Exit();
							}
							catch (Exception e)
							{
								OnError(request);
								Logger.LogError($"Crawler {request.Url} failed: {e}.");
							}
							finally
							{
								_requestedCount.Inc();

								if (_requestedCount.Value % _monitorFlushInterval == 0)
								{
									FlushStatus();
									CheckExitSignal();
								}
							}
						}
					}

					SafeDestroy(downloader);
				});
			}

			IsCompleted = Status == Status.Finished;
			FlushStatus();
			Report(arguments);
			OnClosed?.Invoke(this, IsCompleted);
			OnDispose();
			_exited = true;
		}

		/// <summary>
		/// Pause spider.
		/// </summary>
		/// <param name="action">暂停完成后执行的回调</param>
		public override void Pause(Action action = null)
		{
			var isRunning = Status == Status.Running;
			if (!isRunning)
			{
				Logger.LogWarning(Identity, "Crawler is not running.");
			}
			else
			{
				Status = Status.Paused;
				Logger.LogInformation(Identity, "Stop running...");
			}

			action?.Invoke();
		}

		/// <summary>
		/// Continue spider if spider is paused.
		/// </summary>
		public override void Continue()
		{
			if (Status == Status.Paused)
			{
				Status = Status.Running;
				Logger.LogWarning("Continue...");
			}
			else
			{
				Logger.LogInformation("Crawler was not paused, can not continue...");
			}
		}

		/// <summary>
		/// 发送退出信号
		/// </summary>
		internal void SendExitSignal()
		{
			if (Env.IsWindows)
			{
				var identityMmf = MemoryMappedFile.OpenExisting(Identity, MemoryMappedFileRights.Write);
				using (var stream = identityMmf.CreateViewStream())
				{
					var writer = new BinaryWriter(stream);
					writer.Write(1);
				}

				try
				{
					var taskIdMmf = MemoryMappedFile.OpenExisting(TaskId, MemoryMappedFileRights.Write);
					using (var stream = taskIdMmf.CreateViewStream())
					{
						var writer = new BinaryWriter(stream);
						writer.Write(1);
					}
				}
				catch
				{
					//ignore
				}
			}
			else
			{
				File.Create(_fileCloseSignals[0]);
			}
		}

		/// <summary>
		/// Exit spider.
		/// </summary>
		/// <param name="action">退出完成后执行的回调</param>
		public override void Exit(Action action = null)
		{
			if (Status == Status.Running || Status == Status.Paused)
			{
				Status = Status.Exited;
				Logger.LogInformation("Exit...");
				return;
			}

			Logger.LogWarning(Identity, "Crawler is not running.");
			if (action != null)
			{
				Task.Factory.StartNew(() =>
				{
					while (!_exited)
					{
						Thread.Sleep(100);
					}

					action();
				});
			}
		}

		/// <inheritdoc />
		/// <summary>
		/// Dispose spider.
		/// </summary>
		public void Dispose()
		{
			CheckIfRunning();

			var i = 0;
			while (!_exited)
			{
				++i;
				Thread.Sleep(500);
				if (i > 10)
				{
					break;
				}
			}

			OnDispose();
		}

		/// <summary>
		/// Init component of spider.
		/// </summary>
		/// <param name="arguments"></param>
		protected virtual void InitComponents(params string[] arguments)
		{
			Logger.LogInformation("Init components...");

			if (arguments.Any(a => a?.ToLower() == SpiderArguments.Reset))
			{
				ResetScheduler();
			}

			Downloader = Downloader ?? new HttpClientDownloader();
			Downloader.Logger = Downloader.Logger ?? Logger;

			if (PageProcessors == null || PageProcessors.Count == 0)
			{
				throw new ArgumentException("There is no usable pipeline.");
			}

			foreach (var processor in PageProcessors)
			{
				processor.Logger = processor.Logger ?? Logger;
			}

			InitPipelines(arguments);

			ListenCloseSignals();

			Monitor = Monitor ?? (string.IsNullOrWhiteSpace(Env.HubServiceUrl) ? new LogMonitor() : new HttpMonitor());

			_failingRequestsLogger = new LoggerConfiguration()
				.MinimumLevel.Verbose().WriteTo.RollingFile("failing.log").CreateLogger();

			OnRequestBuilding(arguments);

			LoadScheduler();

			_monitorFlushInterval = CalculateMonitorFlushInterval();

			try
			{
				Console.CancelKeyPress += ConsoleCancelKeyPress;
			}
			catch (Exception e)
			{
				Logger.LogInformation($"Can't register cancel key press:{e.Message}");
			}

			_waitCountLimit = EmptySleepTime / WaitInterval;

			_inited = true;
		}

		protected virtual void ResetScheduler()
		{
			Scheduler.Dispose();
		}

		/// <summary>
		/// Get the default pipeline when user forget set a pepeline to spider.
		/// </summary>
		/// <returns>数据管道</returns>
		protected virtual IPipeline GetDefaultPipeline()
		{
			return new ConsolePipeline();
		}

		/// <summary>
		/// Event when spider on close.
		/// </summary>
		protected void OnDispose()
		{
			var containsData = _cached != null && _cached.Count > 0;

			foreach (var pipeline in Pipelines)
			{
				if (containsData)
				{
					pipeline.Process(_cached.ToArray(), this);
				}

				SafeDestroy(pipeline);
			}

			SafeDestroyScheduler();
			SafeDestroy(PageProcessors);
			SafeDestroy(Downloader);

			if (!Env.IsWindows) return;
			
			foreach (var mmf in _mmfCloseSignals)
			{
				SafeDestroy(mmf);
			}
		}

		/// <summary>
		/// Event when spider on complete.
		/// </summary>
		protected virtual void SafeDestroyScheduler()
		{
			if (ClearSchedulerAfterCompleted && IsCompleted)
			{
				Scheduler.Dispose();
			}
		}

		/// <summary>
		/// Record error request.
		/// </summary>
		/// <param name="request"></param>
		protected void OnError(Request request)
		{
			_failingRequestsLogger.Error($"{Identity} {request}");
			Scheduler.IncreaseErrorCount();
		}

		/// <summary>
		/// Event when spider on success.
		/// </summary>
		protected void OnSuccess(Request request)
		{
			Scheduler.IncreaseSuccessCount();
			OnRequestSucceeded?.Invoke(request);
		}

		/// <summary>
		/// Single/atom logical to handle a request by downloader, processors and pipelines.
		/// </summary>
		/// <param name="stopwatch">计时器</param>
		/// <param name="request">请求信息</param>
		/// <param name="downloader">下载器</param>
		protected Tuple<bool, int, int> HandleRequest(Stopwatch stopwatch, Request request, IDownloader downloader)
		{
			var page = DownloadRequest(stopwatch, request, downloader);

			foreach (var handler in BeforeProcessors)
			{
				handler.Handle(ref page);
			}

			if (page.Content == null)
			{
				// downloader 下载直接构造出的 Page 不会有其它处理, 因此产生的 TargetRequests只可能是重试
				if (page.TargetRequests != null && page.TargetRequests.Count > 0)
				{
					RetriedTimes.Inc();
					ExtractAndAddRequests(page);
				}

				return new Tuple<bool, int, int>(true, 0, 0);
			}

			try
			{
				stopwatch.Reset();
				stopwatch.Start();

				foreach (var processor in PageProcessors)
				{
					processor.Process(page);
				}

				stopwatch.Stop();
				CalculateProcessorSpeed(stopwatch.ElapsedMilliseconds);
			}
			catch (Exception e)
			{
				// 解析异常有可能是下载内容导致的, 也有可能是自己解析写错了, 因此重试。
				if (page.AddToCycleRetry(CycleRetryTimes))
				{
					Logger.LogWarning($"Retry request: {request.Url} because processor exception: {e}.");
				}
				else
				{
					return new Tuple<bool, int, int>(false, 0, 0);
				}
			}

			// Bypass 是最高级别的忽略, 由 Processor修改, 直接忽略 Pipeline 和 TargetRequests
			if (page.Bypass)
			{
				return new Tuple<bool, int, int>(true, 0, 0);
			}

			if (page.Retry)
			{
				RetriedTimes.Inc();
				ExtractAndAddRequests(page);
				return new Tuple<bool, int, int>(false, 0, 0);
			}

			if (!page.ResultItems.IsEmpty || (page.ResultItems.IsEmpty && !SkipTargetRequestsWhenResultIsEmpty))
			{
				ExtractAndAddRequests(page);
			}

			if (page.ResultItems.IsEmpty)
			{
				return new Tuple<bool, int, int>(true, 0, 0);
			}

			stopwatch.Reset();
			stopwatch.Start();

			int countOfResults = 0, effectedRows = 0;

			var resultItems = new ResultItems[0];
			if (PipelineCachedSize == 1)
			{
				resultItems = new[] {page.ResultItems};
			}
			else
			{
				lock (this)
				{
					_cached.Add(page.ResultItems);
					if (_cached.Count >= PipelineCachedSize)
					{
						resultItems = _cached.ToArray();
						_cached.Clear();
					}
				}
			}

			foreach (var pipeline in Pipelines)
			{
				try
				{
					_pipelineRetry.Execute(() => { pipeline.Process(resultItems, this); });
				}
				catch
				{
					Exit();
				}
			}

			foreach (var item in resultItems)
			{
				countOfResults += item.Request.GetCountOfResults();
				effectedRows += item.Request.GetEffectedRows();
			}

			stopwatch.Stop();
			CalculatePipelineSpeed(stopwatch.ElapsedMilliseconds);

			return new Tuple<bool, int, int>(true, countOfResults, effectedRows);
		}

		private Page DownloadRequest(Stopwatch stopwatch, Request request, IDownloader downloader)
		{
			Page page = new Page(request);

			try
			{
				stopwatch.Reset();
				stopwatch.Start();

				page = downloader.Download(request).ToPage();

				stopwatch.Stop();
				CalculateDownloadSpeed(stopwatch.ElapsedMilliseconds);
			}
			catch (BypassedDownloaderException bde)
			{
				Logger.LogError($"Download {request.Url} failed: {bde.Message}.");
				page.Content = null;
			}
			catch (DownloaderException de)
			{
				Logger.LogError($"Download {request.Url} failed: {de.Message}.");
				page.Content = null;
				page.AddToCycleRetry(CycleRetryTimes);
			}
			catch (NeedRedialException re)
			{
				if (NetworkCenter.Current.Executor == null)
				{
					Logger.LogError("RedialExecutor is null.");
					Exit();
				}
				else
				{
					Logger.LogInformation($"Try to redial because: {re.Message}.");
					if (NetworkCenter.Current.Executor.Redial() == RedialResult.Failed)
					{
						Logger.LogError("Redial failed.");
						Exit();
					}
					else
					{
						Logger.LogInformation("Redial success.");
						page.Content = null;
						page.AddToCycleRetry(CycleRetryTimes);
					}
				}
			}
			catch (Exception e)
			{
				Logger.LogError($"Unhandled downloader exception: {e}.");
				Exit();
			}

			return page;
		}

		/// <summary>
		/// Extract and add target urls to scheduler.
		/// </summary>
		/// <param name="page">页面数据</param>
		private void ExtractAndAddRequests(Page page)
		{
			if (page?.Request == null) return;

			if (page.Retry)
			{
				Scheduler.Push(page.Request);
				return;
			}

			if (page.TargetRequests == null || page.TargetRequests.Count == 0) return;

			foreach (var request in page.TargetRequests)
			{
				var depth = request.GetProperty(Page.Depth);
				if (depth == null || depth <= Depth)
				{
					Scheduler.Push(request);
				}
			}
		}

		/// <summary>
		/// Check whether spider is running.
		/// </summary>
		protected override void CheckIfRunning()
		{
			if (Status == Status.Running)
			{
				throw new SpiderException("Spider is running");
			}
		}

		/// <summary>
		/// 初始化数据管道
		/// </summary>
		/// <param name="arguments">运行参数</param>
		protected virtual void InitPipelines(params string[] arguments)
		{
			_cached = new List<ResultItems>(PipelineCachedSize);

			PipelineRetryTimes = PipelineRetryTimes <= 0 ? 1 : PipelineRetryTimes;

			_pipelineRetry = Policy.Handle<Exception>().Retry(PipelineRetryTimes,
				(ex, count) => { Logger.LogError($"Execute pipeline failed [{count}]: {ex}"); });

			if (Pipelines.Count == 0)
			{
				var defaultPipeline = GetDefaultPipeline();
				if (defaultPipeline != null)
				{
					Pipelines.Add(defaultPipeline);
				}
				else
				{
					throw new SpiderException("Count of pipelines should larger than one.");
				}
			}

			foreach (var pipeline in Pipelines)
			{
				pipeline.Logger = pipeline.Logger ?? Logger;
			}
		}

		private void WaitNewUrl(ref int waitCount)
		{
			Thread.Sleep(WaitInterval);
			++waitCount;
		}

		private void SafeDestroy(object obj)
		{
			if (obj == null || !(obj is IDisposable disposable)) return;
			
			try
			{
				disposable.Dispose();
			}
			catch (Exception e)
			{
				Logger.LogError(e.ToString());
			}
		}

		private void ConsoleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			Exit();
			while (!_exited)
			{
				Thread.Sleep(100);
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private void CalculateDownloadSpeed(long time)
		{
			_downloaderTimes++;

			_downloaderCostTimes += time;
			AvgDownloadSpeed = _downloaderCostTimes / _downloaderTimes;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private void CalculateProcessorSpeed(long time)
		{
			_processorTimes++;
			_processorCostTimes += time;
			AvgProcessorSpeed = _processorCostTimes / _processorTimes;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private void CalculatePipelineSpeed(long time)
		{
			_pipelineTimes++;
			_pipelineCostTimes += time;
			AvgPipelineSpeed = _pipelineCostTimes / _pipelineTimes;
		}

		private void OnRequestBuilding(params string[] arguments)
		{
			if (RequestBuilders == null || RequestBuilders.Count <= 0 || !IfRequireRequestBuilders(arguments)) return;
			try
			{
				for (var i = 0; i < RequestBuilders.Count; ++i)
				{
					var builder = RequestBuilders[i];
					Logger.LogInformation($"Add start request via builder[{i + 1}].");
					_requests.AddRange(builder.Build());
				}
			}
			finally
			{
				OnRequestBuildersCompleted();
			}
		}

		private void LoadScheduler()
		{
			if (_requests.Any())
			{
				Logger.LogInformation($"Add start urls to scheduler, count {_requests.Count}.");

				if (!Scheduler.IsDistributed)
				{
					foreach (var request in _requests)
					{
						if (string.IsNullOrWhiteSpace(request.EncodingName))
						{
							request.EncodingName = EncodingName;
						}

						var host = request.RequestUri.Host.ToLower();
						if (_headers.ContainsKey(host))
						{
							foreach (var kv in _headers[host])
							{
								request.Headers.Add(kv.Key, kv.Value);
							}
						}

						Scheduler.Push(request);
					}
				}
				else
				{
					foreach (var request in _requests)
					{
						if (string.IsNullOrWhiteSpace(request.EncodingName))
						{
							request.EncodingName = EncodingName;
						}
					}

					Scheduler.Reload(new HashSet<Request>(_requests));
				}

				// 释放本地内存
				_requests.Clear();
			}
			else
			{
				Logger.LogInformation("Add start urls to scheduler, count 0.");
			}
		}

		private void ListenCloseSignals()
		{
			if (Env.IsWindows)
			{
				_mmfCloseSignals[0] = MemoryMappedFile.CreateOrOpen(Identity, 1, MemoryMappedFileAccess.ReadWrite);
				if (!string.IsNullOrWhiteSpace(TaskId))
				{
					_mmfCloseSignals[1] = MemoryMappedFile.CreateOrOpen(TaskId, 1, MemoryMappedFileAccess.ReadWrite);
				}

				foreach (var mmf in _mmfCloseSignals)
				{
					if (mmf != null)
					{
						using (MemoryMappedViewStream stream = mmf.CreateViewStream())
						{
							var writer = new BinaryWriter(stream);
							writer.Write(false);
						}
					}
				}
			}
			else
			{
				_fileCloseSignals[0] = Path.Combine(Env.BaseDirectory, $"{Identity}_cl");

				if (!string.IsNullOrWhiteSpace(TaskId))
				{
					_fileCloseSignals[1] = Path.Combine(Env.BaseDirectory, $"{TaskId}_cl");
				}

				foreach (var closeSignal in _fileCloseSignals)
				{
					if (File.Exists(closeSignal))
					{
						File.Delete(closeSignal);
					}
				}
			}
		}

		private void FlushStatus()
		{
			try
			{
				Monitor?.Flush(Identity, TaskId, Status.ToString(),
					Monitorable.LeftRequestsCount,
					Monitorable.TotalRequestsCount,
					Monitorable.SuccessRequestsCount,
					Monitorable.ErrorRequestsCount,
					AvgDownloadSpeed,
					AvgProcessorSpeed,
					AvgPipelineSpeed,
					ThreadNum);
			}
			catch (Exception e)
			{
				Logger.LogError($"Report status failed: {e}.");
			}
		}

		/// <summary>
		/// 计算状态监控器每完成多少个Request则上报状态
		/// </summary>
		/// <returns></returns>
		private int CalculateMonitorFlushInterval()
		{
			var leftCount = Scheduler.LeftRequestsCount;
			if (leftCount > 10)
			{
				return 10;
			}

			if (leftCount > 5)
			{
				return 2;
			}

			return 1;
		}

		private void CheckExitSignal()
		{
			// MMF 暂时还不支持非WINDOWS操作系统
			if (Env.IsWindows)
			{
				CheckMmfCloseSignals();
			}
			else
			{
				CheckFileCloseSignals();
			}
		}

		private void CheckMmfCloseSignals()
		{
			foreach (var mmf in _mmfCloseSignals)
			{
				if (mmf != null)
				{
					using (MemoryMappedViewStream stream = mmf.CreateViewStream())
					{
						var reader = new BinaryReader(stream);
						if (reader.ReadBoolean())
						{
							Exit();
							return;
						}
					}
				}
			}
		}

		private void CheckFileCloseSignals()
		{
			if (File.Exists(_fileCloseSignals[0]) || File.Exists(_fileCloseSignals[1]))
			{
				Exit();
			}
		}
	}
}