﻿using System;
using System.Text.RegularExpressions;
using DotnetSpider.Core.Infrastructure;

namespace DotnetSpider.Core.Processor.LastPageChecker
{
	/// <summary>
	/// 最大分页数限制
	/// </summary>
	public class MaxPageLastPageChecker : ILastPageChecker
	{
		private readonly Regex _paginationPattern;
		private readonly int _maxPage;

		/// <summary>
		/// 构造方法
		/// </summary>
		/// <param name="paginationStr">分页信息片段： http://a.com?p=40 PaginationStr: p=40</param>
		/// <param name="maxPage">最大分页数限制</param>
		public MaxPageLastPageChecker(string paginationStr, int maxPage)
		{
			if (string.IsNullOrWhiteSpace(paginationStr))
			{
				throw new ArgumentException("paginationStr is null.");
			}
			_paginationPattern = new Regex($"{RegexUtil.Number.Replace(paginationStr, @"(?<page>\d+)")}");
			_maxPage = maxPage;
		}

		/// <summary>
		/// 是否到了最后一个链接
		/// </summary>
		/// <param name="page">页面数据</param>
		/// <returns>如果返回 True, 则说明已经采到到了最后一个链接</returns>
		public bool IsLastPage(Page page)
		{
			var currentPage = int.Parse(_paginationPattern.Match(page.Request.Url).Groups["page"].Value);
			return currentPage >= _maxPage;
		}
	}
}
