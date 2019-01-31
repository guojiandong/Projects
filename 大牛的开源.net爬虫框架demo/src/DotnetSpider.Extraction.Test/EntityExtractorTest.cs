﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotnetSpider.Extraction.Model;
using DotnetSpider.Extraction.Model.Attribute;
using Xunit;

namespace DotnetSpider.Extraction.Test
{

	public class EntityExtractorTest
	{
		[Fact(DisplayName = "Extract")]
		public void Extract()
		{
			ModelExtractor<Product> extractor = new ModelExtractor<Product>();
			var model = new ModelDefinition<Product>();
			var selectable = new Selectable(
				File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Jd.html")), "http://item.jd.com/3031737.htm");
			selectable.Properties = new Dictionary<string, dynamic> {
				{ "cat", "手机" },
				{ "cat3", "110" }
			};

			var results = extractor.Extract(selectable, model);
			Assert.Equal(60, results.Count());
			Assert.Equal("手机", results.First().CategoryName);
			Assert.Equal(110, results.First().CategoryId);
			Assert.Equal("http://item.jd.com/3031737.html", results.First().Url);
			Assert.Equal("3031737", results.First().Sku);
			Assert.Equal("荣耀官方旗舰店", results.First().ShopName);
			Assert.Equal("荣耀 NOTE 8 4GB+32GB 全网通版 冰河银", results.First().Name);
			Assert.Equal("1000000904", results.First().VenderId);
			Assert.Equal("1000000904", results.First().JdzyShopId);
			Assert.Equal(DateTime.Now.ToString("yyyy-MM-dd"), results.First().RunId.ToString("yyyy-MM-dd"));
		}

		[Entity(Expression = "//li[@class='gl-item']/div[contains(@class,'j-sku-item')]")]
		private class Product : IBaseEntity
		{
			public string AAA;
			private string bb;

			[Field(Expression = "cat", Type = SelectorType.Enviroment)]
			public string CategoryName { get; set; }

			[Field(Expression = "cat3", Type = SelectorType.Enviroment)]
			public int CategoryId { get; set; }

			[Field(Expression = "./div[1]/a/@href")]
			public string Url { get; set; }

			[Field(Expression = "./@data-sku")]
			public string Sku { get; set; }

			[Field(Expression = "./div[5]/strong/a")]
			public long CommentsCount { get; set; }

			[Field(Expression = ".//div[@class='p-shop']/@data-shop_name")]
			public string ShopName { get; set; }

			[Field(Expression = ".//div[@class='p-name']/a/em")]
			public string Name { get; set; }

			[Field(Expression = "./@venderid")]
			public string VenderId { get; set; }

			[Field(Expression = "./@jdzy_shop_id")]
			public string JdzyShopId { get; set; }

			[Field(Expression = "Today", Type = SelectorType.Enviroment)]
			public DateTime RunId { get; set; }
		}
	}
}
