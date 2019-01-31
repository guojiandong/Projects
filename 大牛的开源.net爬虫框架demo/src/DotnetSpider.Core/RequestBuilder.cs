﻿using DotnetSpider.Core.Infrastructure;
using DotnetSpider.Downloader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DotnetSpider.Core
{
	/// <summary>
	/// 起始链接构造器
	/// </summary>
	public interface IRequestBuilder
	{
		/// <summary>
		/// 构造起始链接对象并添加到网站信息对象中
		/// </summary>
		IEnumerable<Request> Build();
	}

	/// <summary>
	/// 递增的起始链接构造器, 可以设置起始数字, 结束数字, 递增间隔, 链接前、后缀
	/// 如: From = 1, To = 10, Interval = 2, Prefix = www.baidu.com/, Postfix  = .html,
	/// 则最终可以构造出: www.baidu.com/1.html, www.baidu.com/3.html, www.baidu.com/5.html, www.baidu.com/7.html, www.baidu.com/9.html
	/// </summary>
	public class ForeachRequestBuilder : IRequestBuilder
	{
		/// <summary>
		/// 递增开始值
		/// </summary>
		public int From { get; }

		/// <summary>
		/// 递增结束值
		/// </summary>
		public int To { get; }

		/// <summary>
		/// 递增间隔
		/// </summary>
		public int Interval { get; }

		/// <summary>
		/// URL拼接前缀
		/// </summary>
		public string Prefix { get; }

		/// <summary>
		/// URL拼接后缀
		/// </summary>
		public string Postfix { get; }

		/// <summary>
		/// 构造方法
		/// </summary>
		/// <param name="min">递增开始值</param>
		/// <param name="max">递增结束值</param>
		/// <param name="interval">递增步进</param>
		/// <param name="prefix">URL拼接前缀</param>
		/// <param name="postfix">URL拼接后缀</param>
		public ForeachRequestBuilder(int min, int max, int interval, string prefix, string postfix)
		{
			From = min;
			To = max;
			Interval = interval;
			Prefix = prefix;
			Postfix = postfix;
		}

		/// <summary>
		/// 构造起始链接对象并添加到网站信息对象中
		/// </summary>
		public IEnumerable<Request> Build()
		{
			var list = new List<Request>();
			for (var i = From; i <= To; i += Interval)
			{
				var request = new Request($"{Prefix}{i}{Postfix}");
				list.Add(request);
			}

			return list;
		}
	}

	/// <summary>
	/// 递增时间的起始链接构造器, 可以设置起始时间, 结束时间, 时间格式化字符串, 递增间隔, 链接前、后缀
	/// 如: From = 2017-01-01, To = 2017-01-10, Interval = 1, Prefix = www.baidu.com/, Postfix  = .html, DateFormateString = yyyy-MM-dd
	/// 则最终可以构造出: www.baidu.com/2017-01-01.html, www.baidu.com/2017-01-02.html, www.baidu.com/2017-01-03.html...
	/// </summary>
	public class ForeachDateRequestBuilder : IRequestBuilder
	{
		/// <summary>
		/// 递增起始时间
		/// </summary>
		public DateTime From { get; }

		/// <summary>
		/// 递增结束时间
		/// </summary>
		public DateTime To { get; }

		/// <summary>
		/// 递增间隔(天)
		/// </summary>
		public int Interval { get; }

		/// <summary>
		/// 时间格式化字符串
		/// </summary>
		public string DateFormat { get; }

		/// <summary>
		/// URL拼接前缀
		/// </summary>
		public string Prefix { get; }

		/// <summary>
		/// URL拼接后缀
		/// </summary>
		public string Postfix { get; }

		/// <summary>
		/// 构造方法
		/// </summary>
		/// <param name="from">递增起始时间</param>
		/// <param name="to">递增结束时间</param>
		/// <param name="interval">递增间隔(天)</param>
		/// <param name="prefix">URL拼接前缀</param>
		/// <param name="postfix">URL拼接后缀</param>
		/// <param name="dateFormat">时间格式化字符串</param>
		public ForeachDateRequestBuilder(DateTime from, DateTime to, int interval, string prefix, string postfix,
			string dateFormat = "yyyy-MM-dd")
		{
			From = from;
			To = to;
			Interval = interval;
			Prefix = prefix;
			Postfix = postfix;
			DateFormat = dateFormat;
		}

		/// <summary>
		/// 构造起始链接对象并添加到网站信息对象中
		/// </summary>
		public IEnumerable<Request> Build()
		{
			List<Request> list = new List<Request>();
			for (var i = From; i <= To; i = i.AddDays(Interval))
			{
				var date = i.ToString(DateFormat);
				var request = new Request($"{Prefix}{date}{Postfix}");
				list.Add(request);
			}

			return list;
		}
	}

	/// <summary>
	/// 起始链接构造器
	/// </summary>
	public class DatabaseRequestBuilder : IRequestBuilder
	{
		private readonly ConnectionStringSettings _connectionStringSettings;

		private readonly string _sql;

		/// <summary>
		/// 拼接Url的方式, 会把Columns对应列的数据传入
		/// https://s.taobao.com/search?q={0},s=0;
		/// </summary>
		private readonly string[] _formats;

		private readonly string[] _formatArguments;

		/// <summary>
		/// 从数据库中查询出的结果可以先做一下格式
		/// </summary>
		/// <param name="item">数据对象</param>
		protected virtual void FormatDataObject(IDictionary<string, object> item)
		{
		}

		/// <summary>
		/// 格式化最终的请求信息
		/// </summary>
		/// <param name="request">请求信息</param>
		protected virtual void FormatRequest(Request request)
		{
		}
		
		/// <summary>
		/// 查询数据库结果
		/// </summary>
		/// <returns>数据库结果</returns>
		protected List<Dictionary<string, dynamic>> QueryData()
		{
			var list = new List<Dictionary<string, dynamic>>();
			using (var conn = _connectionStringSettings.CreateDbConnection())
			{
				foreach (var item in conn.MyQuery(_sql))
				{
					var dic =
						((IDictionary<string, dynamic>) item).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
					FormatDataObject(dic);
					list.Add(dic);
				}
			}

			return list;
		}

		/// <summary>
		/// 构造方法
		/// </summary>
		/// <param name="sql">SQL 语句</param>
		/// <param name="formatArguments">起始链接格式化参数</param>
		/// <param name="formats">起始链接格式化模版</param>
		public DatabaseRequestBuilder(string sql, string[] formatArguments, params string[] formats)
		{
			if (Env.DataConnectionStringSettings == null)
			{
				throw new SpiderException("DataConnection is unfounded in app.config");
			}

			_connectionStringSettings = Env.DataConnectionStringSettings;
			_sql = sql;
			_formats = formats;
			_formatArguments = formatArguments;
		}

		/// <summary>
		/// 构造方法
		/// </summary>
		/// <param name="source">数据库类型</param>
		/// <param name="connectString">数据库连接字符串</param>
		/// <param name="sql">SQL 语句</param>
		/// <param name="formatArguments">起始链接格式化参数</param>
		/// <param name="formats">起始链接格式化模版</param>
		public DatabaseRequestBuilder(Database source, string connectString, string sql, string[] formatArguments,
			params string[] formats)
		{
			switch (source)
			{
				case Database.MySql:
				{
					_connectionStringSettings =
						new ConnectionStringSettings("MySql", connectString, "MySql.Data.MySqlClient");
					break;
				}
				case Database.SqlServer:
				{
					_connectionStringSettings =
						new ConnectionStringSettings("SqlServer", connectString, "System.Data.SqlClient");
					break;
				}
				default:
				{
					throw new SpiderException($"Database {source} is unsupported right now");
				}
			}

			_sql = sql;
			_formats = formats;
			_formatArguments = formatArguments;
		}

		/// <summary>
		/// 构造起始链接对象并添加到网站信息对象中
		/// </summary>
		public IEnumerable<Request> Build()
		{
			var data = QueryData();

			var list = new List<Request>();
			foreach (var item in data)
			{
				object[] arguments = _formatArguments.Select(a => item[a]).ToArray();

				foreach (var format in _formats)
				{
					var url = string.Format(format, arguments);
					var request = new Request(url, item);
					FormatRequest(request);
					list.Add(request);
				}
			}

			return list;
		}
	}
}