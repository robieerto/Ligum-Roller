using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ligum_Roller.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ligum_Roller.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class DataController : ControllerBase
	{
		private readonly ILogger _logger;

		public DataController(ILogger<DataController> logger)
		{
			_logger = logger;
		}

		// POST data
		[HttpPost]
		public async Task<string> Post()
		{
			_logger.LogInformation("Received data {verb}", "POST");
			//Console.WriteLine(GetAllHeadersStr());
			using StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
			var bodyData = await reader.ReadToEndAsync();
			var stringLines = bodyData.Split('\n');
			// at least 2 lines - header and timestamp are expected
			if (stringLines.Length < 2)
			{
				_logger.LogError("Wrong format of data");
				return "Bad request";
			}
			//var timestamp = stringLines.Last();
			var timestamp = DataLayer.GetCurrentDateTimeStr();
			var csvData = bodyData.Remove(bodyData.LastIndexOf('\n'));

			if (DataLayer.ParseCsv(csvData) == null)
			{
				_logger.LogError("Wrong format of data");
				return "Wrong format";
			}

			// save data async
			_ = DataLayer.SaveRecord(csvData, timestamp, _logger);

			return "OK";
		}

		[HttpGet]
		public string Get()
		{
			return "Data endpoint";
		}
	}
}
