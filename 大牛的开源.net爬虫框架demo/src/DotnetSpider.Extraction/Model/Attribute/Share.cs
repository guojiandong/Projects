﻿using System;

namespace DotnetSpider.Extraction.Model.Attribute
{
	/// <summary>
	/// 共享值的查询器定义. 只能申明在一个爬虫实体类上面. 此查询器的结果会添加到 Environment 中, 即在爬虫实体类中的属性定义 EnvironmentSelector 可以获取到共享值.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class Share : Selector
	{
		/// <summary>
		/// 共享值的名称
		/// </summary>
		public string Name { get; set; }
	}
}
