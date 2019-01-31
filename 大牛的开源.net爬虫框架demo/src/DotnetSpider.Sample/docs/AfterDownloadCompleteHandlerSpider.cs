﻿using DotnetSpider.Downloader;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Pipeline;
using DotnetSpider.Extraction;
using DotnetSpider.Extraction.Model;
using DotnetSpider.Extraction.Model.Attribute;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DotnetSpider.Sample.docs
{
	public class AfterDownloadCompleteHandlerSpider
	{
		public static void Run()
		{
			SinaNewsSpider spider = new SinaNewsSpider();
			spider.Run();
		}

		private class SinaNewsSpider : EntitySpider
		{
			public SinaNewsSpider()
			{
			}

			protected override void OnInit(params string[] arguments)
			{
				AddRequest($"http://api.search.sina.com.cn/?c=news&t=&q=赵丽颖&pf=2136012948&ps=2130770082&page=0&stime={DateTime.Now.AddYears(-7).AddDays(-1).ToString("yyyy-MM-dd")}&etime={DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")}&sort=rel&highlight=1&num=10&ie=utf-8&callback=jQuery1720001955628746606708_1508996230766&_=1508996681484", new Dictionary<string, dynamic> { { "keyword", "赵丽颖" } });
				AddPipeline(new ConsoleEntityPipeline());
				Downloader = new HttpClientDownloader();
				Downloader.AddAfterDownloadCompleteHandler(new ReplaceHandler());
				AddEntityType<SinaNews>();
			}

			class ReplaceHandler : AfterDownloadCompleteHandler
			{
				public override void Handle(ref Response page, IDownloader downloader)
				{
					page.Content = page.Content?.ToString().Replace("jQuery1720001955628746606708_1508996230766(", "").Replace("});", "}");
					page.Content = ClearHtml(page.Content?.ToString());
				}

				/// <summary>  
				/// 清除文本中Html的标签  
				/// </summary>  
				/// <param name="Content"></param>  
				/// <returns></returns>  
				protected string ClearHtml(string Content)
				{
					Content = Zxj_ReplaceHtml("&#[^>]*;", "", Content);
					Content = Zxj_ReplaceHtml("</?marquee[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?object[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?param[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?embed[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?table[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml(" ", "", Content);
					Content = Zxj_ReplaceHtml("</?tr[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?th[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?p[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?a[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?img[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?tbody[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?li[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?span[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?div[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?th[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?td[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?script[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("(javascript|jscript|vbscript|vbs):", "", Content);
					Content = Zxj_ReplaceHtml("on(mouse|exit|error|click|key)", "", Content);
					Content = Zxj_ReplaceHtml("<\\?xml[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("<\\/?[a-z]+:[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?font[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?b[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?u[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?i[^>]*>", "", Content);
					Content = Zxj_ReplaceHtml("</?strong[^>]*>", "", Content);
					string clearHtml = Content;
					return clearHtml;
				}

				/// <summary>  
				/// 清除文本中的Html标签  
				/// </summary>  
				/// <param name="patrn">要替换的标签正则表达式</param>  
				/// <param name="strRep">替换为的内容</param>  
				/// <param name="content">要替换的内容</param>  
				/// <returns></returns>  
				private string Zxj_ReplaceHtml(string patrn, string strRep, string content)
				{
					if (string.IsNullOrEmpty(content))
					{
						content = "";
					}
					Regex rgEx = new Regex(patrn, RegexOptions.IgnoreCase);
					string strTxt = rgEx.Replace(content, strRep);
					return strTxt;
				}
			}

			[Entity(Expression = "$.result.list[*]", Type = SelectorType.JsonPath)]
			class SinaNews : BaseEntity
			{
				[Field(Expression = "$.origin_title", Type = SelectorType.JsonPath, Option = FieldOptions.InnerText)]
				public string Title { get; set; }

				[Field(Expression = "$.url", Type = SelectorType.JsonPath)]
				public string Link { get; set; }

				[Field(Expression = "keyword", Type = SelectorType.Enviroment)]
				public string Keywords { get; set; }

				[Field(Expression = "$.intro", Type = SelectorType.JsonPath, Option = FieldOptions.InnerText)]
				public string Summary { get; set; }

				[Field(Expression = "$.media", Type = SelectorType.JsonPath)]
				public string NewsFrom { get; set; }

				[Field(Expression = "$.datetime", Type = SelectorType.JsonPath)]
				public string PublishTime { get; set; }

				[Field(Expression = "$.cid", Type = SelectorType.JsonPath)]
				public string Cid { get; set; }
			}
		}
	}
}
