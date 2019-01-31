using System.Collections.Generic;

namespace DotnetSpider.Core.Pipeline
{
	/// <summary>
	/// 打印数据结果到控制台
	/// </summary>
	public class ConsolePipeline : BasePipeline
	{
		/// <summary>
		/// 打印数据结果到控制台
		/// </summary>
		/// <param name="resultItems">数据结果</param>
		/// <param name="sender">调用方</param>
		public override void Process(IList<ResultItems> resultItems, dynamic sender = null)
		{
			foreach (var resultItem in resultItems)
			{
				foreach (var entry in resultItem)
				{
					System.Console.WriteLine(entry.Key + ":\t" + entry.Value);

					resultItem.Request.AddCountOfResults(1);
					resultItem.Request.AddEffectedRows(1);
				}
			}
		}
	}
}
