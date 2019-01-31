using DotnetSpider.Core.Monitor;
using DotnetSpider.Core.Scheduler;
using DotnetSpider.Downloader;
using System;

namespace DotnetSpider.Core
{
	/// <summary>
	/// 爬虫接口定义
	/// </summary>
	public interface ISpider : IDisposable, IAppBase
	{
		IScheduler Scheduler { get; }

		/// <summary>
		/// 下载器
		/// </summary>
		IDownloader Downloader { get; set; }

		/// <summary>
		/// 监控接口
		/// </summary>
		IMonitor Monitor { get; set; }
	}
}
