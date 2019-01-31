﻿using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Scheduler;
using Xunit;
using System.Collections.Generic;
using DotnetSpider.Downloader;

namespace DotnetSpider.Core.Test
{
	public class ProcessorTest
	{
		[Fact(DisplayName = "RetryProcessor")]
		public void ProcesserException()
		{
			var scheduler = new QueueDuplicateRemovedScheduler();
			Spider spider = Spider.Create(
				// crawler identity
				"youku",
				// use memoery queue scheduler
				scheduler,
				// default page processor will save whole html, and extract urls to target urls via regex
				new TestPageProcessor())
				// save crawler result to file in the folder: \{running directory}\data\{crawler identity}\{guid}.dsd
				.AddPipeline(new FilePipeline());
			spider.ClearSchedulerAfterCompleted = false;
			// dowload html by http client
			spider.Downloader = new HttpClientDownloader();
			spider.AddHeaders("v.youku.com", new Dictionary<string, object> { { "Upgrade-Insecure-Requests", "1" } });

			spider.ThreadNum = 1;
			// traversal deep 遍历深度
			spider.Depth = 3;
			spider.EmptySleepTime = 6000;

			spider.AddRequests("http://v.youku.com/v_show/id_XMTMyMTkzNTY1Mg==.html?spm=a2h1n.8251845.0.0");
			spider.AddRequests("http://v.youku.com/v_show/id_XMjkzNzMwMDMyOA==.html?spm=a2h1n.8251845.0.0");
			spider.AddRequests("http://v.youku.com/v_show/id_XMjcwNDg0NDI3Mg==.html?spm=a2h1n.8251845.0.0");
			spider.AddRequests("http://v.youku.com/v_show/id_XMTMwNzQwMTcwMA==.html?spm=a2h1n.8251845.0.0");
			spider.AddRequests("http://v.youku.com/v_show/id_XMjk1MzI0Mzk4NA==.html?spm=a2h1n.8251845.0.0");
			spider.AddRequests("http://v.youku.com/v_show/id_XMjkzNzY0NzkyOA==.html?spm=a2h1n.8251845.0.0");
			spider.AddRequests("http://www.163.com/");

			// start crawler 启动爬虫
			spider.Run();

			Assert.Equal(5, spider.RetriedTimes.Value);
			Assert.Equal(0, scheduler.LeftRequestsCount);
			Assert.Equal(6, scheduler.SuccessRequestsCount);
			Assert.Equal(6, scheduler.ErrorRequestsCount);
		}

		class TestPageProcessor : BasePageProcessor
		{
			protected override void Handle(Page page)
			{
				if (page.Request.Url.ToString() == "http://www.163.com/")
				{
					throw new SpiderException("");
				}
			}
		}
	}
}
