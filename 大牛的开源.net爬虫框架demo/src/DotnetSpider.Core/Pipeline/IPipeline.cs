using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DotnetSpider.Core.Pipeline
{
	/// <summary>
	/// 数据管道接口, 通过数据管道把解析的数据存到不同的存储中(文件、数据库）
	/// </summary>
	public interface IPipeline : IDisposable
	{
		ILogger Logger { get; set; }

		/// <summary>
		/// 处理页面解析器解析到的数据结果
		/// </summary>
		/// <param name="resultItems">数据结果</param>
		/// <param name="sender">调用方</param>
		void Process(IList<ResultItems> resultItems, dynamic sender = null);
	}
}