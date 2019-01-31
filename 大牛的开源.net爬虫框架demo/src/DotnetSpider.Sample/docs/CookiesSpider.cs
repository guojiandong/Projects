﻿using DotnetSpider.Downloader;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Pipeline;
using DotnetSpider.Extraction.Model;
using DotnetSpider.Extraction.Model.Attribute;
using DotnetSpider.Extraction.Model.Formatter;
using System.Collections.Generic;

namespace DotnetSpider.Sample.docs
{
	public class CookiesSpider : EntitySpider
	{
		public CookiesSpider()
		{
		}

		protected override void OnInit(params string[] arguments)
		{
			Downloader.AddCookies("sid=dea284fc36c24e8cbcd447343d7b8a4e; sn=DD962248; ctid=000000; ctnm=%E5%8F%A4%E9%95%87%E7%81%AF%E9%A5%B0%E6%89%B9%E5%8F%91; ctpv=%E5%B9%BF%E4%B8%9C; JSESSIONID=acbBqFfOD4I63d9PziDvv; DDENG=c4fc08ae2e3ba3efeddbc667c2f45e615a85e80009169501dc244a03e87908aa61146548b97ed9c7dc07af23bfd80bff5008f8c8867a9165d4bd2732aca0db7dedae2e042d3968fcad1150f36be242e8a32a3f59db2a0b39216a59f1628508c5799644532a9d99925f9841b3c13a1f97; userId=10003379; previousUser=%E5%A4%95%E7%8E%89; Hm_lvt_9e33f153f28be198970d205d90a24f28=1466146335; Hm_lpvt_9e33f153f28be198970d205d90a24f28=1466146392; Hm_lvt_54b4cb498afd05463ab4611b38a6f289=1466146335; Hm_lpvt_54b4cb498afd05463ab4611b38a6f289=1466146392; CNZZDATA1256982382=395301521-1466143554-%7C1466143554",
					"www.ddeng.com");

			AddRequest(new Request("http://www.ddeng.com/product/982227")
			{
				Headers = new Dictionary<string, object>
				{
					{ "Cache-Control","max-age=0"},
					{ "Upgrade-Insecure-Requests","1"}
				},
				UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36",
				Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"
			});
			AddPipeline(new ConsoleEntityPipeline());
			AddEntityType<Corp>();
		}

		[Schema("test", "ddeng_corp", TableNamePostfix.Today)]
		class Corp : IBaseEntity
		{
			[Column]
			[Field(Expression = "/html/body/div[4]/div[2]/div[3]/div[1]/p[1]/strong")]
			public string Name { get; set; }

			[ReplaceFormatter(NewValue = "", OldValue = "\r")]
			[ReplaceFormatter(NewValue = "", OldValue = "\t")]
			[ReplaceFormatter(NewValue = "", OldValue = "&nbsp;")]
			[ReplaceFormatter(NewValue = "", OldValue = "\n")]
			[ReplaceFormatter(NewValue = "", OldValue = "\"")]
			[ReplaceFormatter(NewValue = "", OldValue = " ")]
			[Field(Expression = "/html/body/div[4]/div[2]/div[3]/div[1]/ul/li[2]/div", Option = FieldOptions.InnerText)]
			[Column]
			public string Phone { get; set; }

			[ReplaceFormatter(NewValue = "", OldValue = "\r")]
			[ReplaceFormatter(NewValue = "", OldValue = "\t")]
			[ReplaceFormatter(NewValue = "", OldValue = "&nbsp;")]
			[ReplaceFormatter(NewValue = "", OldValue = "\n")]
			[ReplaceFormatter(NewValue = "", OldValue = "\"")]
			[ReplaceFormatter(NewValue = "", OldValue = " ")]
			[ReplaceFormatter(NewValue = "", OldValue = "地址：")]
			[Column]
			[Field(Expression = "/html/body/div[4]/div[2]/div[3]/div[1]/ul/li[3]", Option = FieldOptions.InnerText)]
			public string Address { get; set; }

			[Field(Expression = ".")]
			[Column(Length = 0)]
			public string Html { get; set; }
		}
	}
}
