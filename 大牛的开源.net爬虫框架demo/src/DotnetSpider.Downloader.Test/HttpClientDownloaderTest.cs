﻿using System.Text;
using System.Threading.Tasks;
using Xunit;
#if !NETFRAMEWORK

#endif

namespace DotnetSpider.Downloader.Test
{
	public class HttpClientDownloaderTest
	{
		public HttpClientDownloaderTest()
		{
#if !NETFRAMEWORK
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif

		}

		[Fact(DisplayName = "ParallelDownloader")]
		public void ParallelDownloader()
		{	var downloader=new HttpClientDownloader();
			Parallel.For(0, 3, new ParallelOptions
			{
				MaxDegreeOfParallelism = 3
			}, i =>
			{
				var d = downloader.Clone();
				downloader.Download(new Request("http://www.163.com"));
			});
		}

		/// <summary>
		/// 手动执行此测试脚本，运行结束后用netstat -ano 查看端口占用情况。只会占用一个就对了
		/// </summary>
		[Fact(Skip = "Need person double check", DisplayName = "Ports")]
		public void Ports()
		{
			HttpClientDownloader downloader = new HttpClientDownloader();

			for (int i = 0; i < 100; i++)
			{
				var a = downloader.Download(new Request("http://www.163.com", null));
			}
		}

		[Fact(DisplayName = "DetectDownloadContent")]
		public void DetectDownloadContent()
		{
			HttpClientDownloader downloader = new HttpClientDownloader();

			var a = downloader.Download(new Request("http://www.163.com", null));
			Assert.Equal(ContentType.Html, a.ContentType);

			HttpClientDownloader2 downloader2 = new HttpClientDownloader2();
			a = downloader2.Download(new Request("http://www.163.com", null));
			Assert.Equal(ContentType.Json, a.ContentType);
		}

		//[Fact(DisplayName = "_404Url")]
		//public void _404Url()
		//{
		//	if (!Env.IsWindows)
		//	{
		//		return;
		//	}
		//	var spider = Spider.Create(new Site { EncodingName = "UTF-8", SleepTime = 1000 },
		//			"abcd",
		//			new QueueDuplicateRemovedScheduler(),
		//			new TestPageProcessor());
		//	spider.AddPipeline(new ConsolePipeline());
		//	spider.SkipTargetUrlsWhenResultIsEmpty = false;
		//	spider.EmptySleepTime = 6000;
		//	spider.AddRequest("http://www.mlr.gov.cn/xwdt/jrxw/201707/t20170710_15242382.htm");
		//	spider.Run();
		//	Assert.Equal(5, spider.RetriedTimes.Value);
		//}

		//[Fact(DisplayName = "_301Url")]
		//public void _301Url()
		//{
		//	if (!Env.IsWindows)
		//	{
		//		return;
		//	}
		//	var spider = Spider.Create(new Site { EncodingName = "UTF-8", SleepTime = 1000 },
		//			"abcd",
		//			new QueueDuplicateRemovedScheduler(),
		//			new TestPageProcessor());
		//	spider.AddPipeline(new ConsolePipeline());
		//	spider.SkipTargetUrlsWhenResultIsEmpty = true;
		//	spider.Downloader = new HttpClientDownloader();
		//	spider.EmptySleepTime = 6000;
		//	spider.AddRequest("https://tieba.baidu.com/f?kw=%E7%AE%80%E9%98%B3&ie=utf-8&pn=50");
		//	spider.Run();
		//	Assert.Equal(0, spider.RetriedTimes.Value);
		//}

		class HttpClientDownloader2 : HttpClientDownloader
		{
			protected override Response DownloadContent(Request request)
			{
				var page = new Response(request) { Content = "{'a':'b'}" };
				DetectContentType(page, null);
				return page;
			}
		}

		[Fact(DisplayName = "GetTargetUrlWhenRedirect")]
		public void GetTargetUrlWhenRedirect()
		{
			var downloader = new HttpClientDownloader();
			var page = downloader.Download(new Request("http://item.jd.com/1231222221111123.html", null));
			Assert.DoesNotContain("1231222221111123", page.TargetUrl);
			Assert.True(page.TargetUrl.Contains("www.jd.com/") || page.TargetUrl.Contains("global.jd.com"));
		}
	}
}
