﻿using DotnetSpider.Core;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Processor.Filter;
using DotnetSpider.Core.Processor.RequestExtractor;
using DotnetSpider.Core.Scheduler;
using DotnetSpider.Extraction;
using System;
using System.Collections.Generic;

namespace DotnetSpider.Sample.docs
{
	/// <summary>
	/// 通过设置 Processor 中的 TargetUrlsExtractor 属性, 让不同的 URL 进入到不同的 Processor 中做解析
	/// </summary>
	public class MultiProcessorsSpider
	{
		public static void Run()
		{
			// 使用内存Scheduler、自定义PageProcessor、自定义Pipeline创建爬虫
			Spider spider = Spider.Create(
				new QueueDuplicateRemovedScheduler(),
				new BlogSumaryProcessor(),
				new NewsProcessor()).
				AddPipeline(new MyPipeline());
			spider.EncodingName = "UTF-8";
			for (int i = 1; i < 5; ++i)
			{
				// 添加初始采集链接
				spider.AddRequests("http://www.cnblogs.com");
			}
			// 启动爬虫
			spider.Run();
		}

		private class MyPipeline : BasePipeline
		{
			private static long blogSumaryCount = 0;
			private static long newsCount = 0;

			public override void Process(IList<ResultItems> resultItems, dynamic sender = null)
			{
				foreach (var resultItem in resultItems)
				{
					if (resultItem["BlogSumary"] != null)
					{
						foreach (BlogSumary entry in (List<BlogSumary>)resultItem["BlogSumary"])
						{
							blogSumaryCount++;
							Console.WriteLine($"BlogSumary [{blogSumaryCount}] {entry}");
						}
					}

					if (resultItem["News"] != null)
					{
						foreach (News entry in (List<News>)resultItem["News"])
						{
							newsCount++;
							Console.WriteLine($"News [{newsCount}] {entry}");
						}
					}
				}
				// 可以自由实现插入数据库或保存到文件
			}
		}

		private class BlogSumaryProcessor : BasePageProcessor
		{
			public BlogSumaryProcessor()
			{
				RequestExtractor = new XPathRequestExtractor(".");
				Filter = new PatternFilter(new[] { "^http://www\\.cnblogs\\.com/$", "http://www\\.cnblogs\\.com/sitehome/p/\\d+" });
			}

			protected override void Handle(Page page)
			{
				// 利用 Selectable 查询并构造自己想要的数据对象
				var blogSummaryElements = page.Selectable().SelectList(Selectors.XPath("//div[@class='post_item']")).Nodes();
				List<BlogSumary> results = new List<BlogSumary>();
				foreach (var blogSummary in blogSummaryElements)
				{
					var video = new BlogSumary();
					video.Name = blogSummary.Select(Selectors.XPath(".//a[@class='titlelnk']")).GetValue();
					video.Url = blogSummary.Select(Selectors.XPath(".//a[@class='titlelnk']/@href")).GetValue();
					video.Author = blogSummary.Select(Selectors.XPath(".//div[@class='post_item_foot']/a[1]")).GetValue();
					video.PublishTime = blogSummary.Select(Selectors.XPath(".//div[@class='post_item_foot']/text()")).GetValue();
					results.Add(video);
				}

				// 以自定义KEY存入page对象中供Pipeline调用
				page.AddResultItem("BlogSumary", results);
			}
		}

		private class NewsProcessor : BasePageProcessor
		{
			public NewsProcessor()
			{
				Filter = new PatternFilter(new[] { "^http://www\\.cnblogs\\.com/$", "^http://www\\.cnblogs\\.com/news/$", "www\\.cnblogs\\.com/news/\\d+" });
			}

			protected override void Handle(Page page)
			{
				// 利用 Selectable 查询并构造自己想要的数据对象
				var newsElements = page.Selectable().SelectList(Selectors.XPath("//div[@class='post_item']")).Nodes();
				List<News> results = new List<News>();
				foreach (var news in newsElements)
				{
					var video = new News();
					video.Name = news.Select(Selectors.XPath(".//a[@class='titlelnk']")).GetValue();
					video.Url = news.Select(Selectors.XPath(".//a[@class='titlelnk']/@href")).GetValue();
					video.PublishTime = news.Select(Selectors.XPath(".//div[@class='post_item_foot']/text()")).GetValue();
					results.Add(video);
				}

				// 以自定义KEY存入page对象中供Pipeline调用
				page.AddResultItem("News", results);
			}
		}

		class BlogSumary
		{
			public string Name { get; set; }
			public string Author { get; set; }
			public string PublishTime { get; set; }
			public string Url { get; set; }

			public override string ToString()
			{
				return $"{Name}|{Author}|{PublishTime}|{Url}";
			}
		}

		class News
		{
			public string Name { get; set; }
			public string PublishTime { get; set; }
			public string Url { get; set; }
			public override string ToString()
			{
				return $"{Name}|{PublishTime}|{Url}";
			}
		}
	}
}
