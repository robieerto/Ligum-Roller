using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ligum_Roller
{
	public abstract class ControllerBase : Controller
	{
		public Dictionary<string, string> GetAllHeaders()
		{
			Dictionary<string, string> requestHeaders =
			   new Dictionary<string, string>();
			foreach (var header in Request.Headers)
			{
				requestHeaders.Add(header.Key, header.Value);
			}
			return requestHeaders;
		}

		public string GetAllHeadersStr()
		{
			var lines = GetAllHeaders().Select(a => $"{a.Key}: {a.Value}");
			return string.Join(Environment.NewLine, lines);
		}
	}
}
