﻿using DotnetSpider.Core.Infrastructure;
using System.Configuration;
using System.Linq;

namespace DotnetSpider.Extension.Infrastructure
{
	/// <summary>
	/// 从中间数据库中获取数据库连接设置的实现, 此功能用在使用一个公用数据库存储实际数据库连接字符串, 当实际数据库的用户名密码有变时, 则把新的
	/// 连接字符串更新到公用数据库中, 则实现所有爬虫实际更新的功能
	/// </summary>
	public class DbConnectionStringSettingsRefresher : IConnectionStringSettingsRefresher
	{
		/// <summary>
		/// 连接字符串
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		/// 数据库类型
		/// </summary>
		public Database DataSource { get; set; } = Database.MySql;

		/// <summary>
		/// 查询的SQL语句
		/// </summary>
		public string QueryString { get; set; }

		/// <summary>
		/// 获取新的数据库连接设置
		/// </summary>
		/// <returns>数据库连接设置</returns>
		public ConnectionStringSettings GetNew()
		{
			using (var conn = DatabaseExtensions.CreateDbConnection(DataSource, ConnectionString))
			{
				ConnectionStringSettings connectString =
					conn.MyQuery<ConnectionStringSettings>(QueryString).FirstOrDefault();
				return connectString;
			}
		}
	}
}