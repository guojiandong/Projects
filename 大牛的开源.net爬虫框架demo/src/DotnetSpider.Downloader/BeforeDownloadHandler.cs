﻿namespace DotnetSpider.Downloader
{
	/// <summary>
	/// Pre-process before downloading, you can edit request here like PostBody etc.
	/// </summary>
	/// <summary xml:lang="zh-CN">
	/// 下载工作的预处理, 可以在执行下载前替换关键信息: 如修正PostBody
	/// </summary>
	public abstract class BeforeDownloadHandler :  IBeforeDownloadHandler
	{
		/// <summary>
		/// Pre-process before downloading
		/// </summary>
		/// <summary xml:lang="zh-CN">
		/// 下载工作的预处理
		/// </summary>
		/// <param name="request">请求信息 <see cref="Request"/></param>
		/// <param name="downloader">下载器 <see cref="IDownloader"/></param>
		public abstract void Handle(ref Request request, IDownloader downloader);
	}
}
