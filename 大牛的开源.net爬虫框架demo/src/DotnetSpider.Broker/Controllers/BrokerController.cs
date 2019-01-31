﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotnetSpider.Broker.Controllers
{
	public abstract class BrokerController : ControllerBase
	{
		protected readonly ILogger _logger;
		protected readonly BrokerOptions _options;

		public BrokerController(ILogger<BrokerController> logger, BrokerOptions options)
		{
			_logger = logger;
			_options = options;
		}

		protected string GetRemoveIpAddress()
		{
			return HttpContext.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
		}
	}
}
