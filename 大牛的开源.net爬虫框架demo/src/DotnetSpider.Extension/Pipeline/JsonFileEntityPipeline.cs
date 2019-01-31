﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using DotnetSpider.Core;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extraction.Model;
using Newtonsoft.Json;

namespace DotnetSpider.Extension.Pipeline
{
	/// <summary>
	/// 把解析到的爬虫实体数据序列化成JSON并存到文件中
	/// </summary>
	public class JsonFileEntityPipeline : EntityPipeline
	{
		private readonly Dictionary<string, StreamWriter> _writers = new Dictionary<string, StreamWriter>();

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public override void Dispose()
		{
			base.Dispose();
			foreach (var writer in _writers)
			{
				writer.Value.Dispose();
			}
		}

		/// <summary>
		/// 把解析到的爬虫实体数据序列化成JSON并存到文件中
		/// </summary>
		/// <param name="items">实体类数据</param>
		/// <param name="sender">调用方</param>
		/// <returns>最终影响结果数量(如数据库影响行数)</returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		protected override int Process(List<IBaseEntity> items, dynamic sender = null)
		{
			if (items == null || !items.Any()) return 0;
	
			var tableInfo = new TableInfo(items.First().GetType());

			StreamWriter writer;
			var identity = GetIdentity(sender);
			var dataFolder = Path.Combine(Env.BaseDirectory, "json", identity);
			var jsonFile = Path.Combine(dataFolder, $"{tableInfo.Schema.FullName}.json");
			if (_writers.ContainsKey(jsonFile))
			{
				writer = _writers[jsonFile];
			}
			else
			{
				if (!Directory.Exists(dataFolder))
				{
					Directory.CreateDirectory(dataFolder);
				}
				writer = new StreamWriter(File.OpenWrite(jsonFile), Encoding.UTF8);
				_writers.Add(jsonFile, writer);
			}

			foreach (var entry in items)
			{
				writer.WriteLine(JsonConvert.SerializeObject(entry));
			}
			return items.Count;
		}
	}
}
