using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ligum_Roller.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ligum_Roller.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class DataController : ControllerBase
	{
		// POST data
		[HttpPost]
		public async Task<string> Post()
		{
			//Console.WriteLine(GetAllHeadersStr());
			using StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
			var bodyData = await reader.ReadToEndAsync();
			var stringLines = bodyData.Split('\n');
			// at least 2 lines - header and timestamp are expected
			if (stringLines.Length < 2)
			{
				return "Bad request";
			}
			//var timestamp = stringLines.Last();
			var timestamp = DataLayer.GetCurrentDateTimeStr();
			var csvData = bodyData.Remove(bodyData.LastIndexOf('\n'));
			Console.WriteLine(timestamp);
			// save data
			await DataLayer.SaveRecord(csvData, timestamp);

			return "OK";
		}

		[HttpGet]
		public string Get()
		{
			return "OK";
		}
	}
}
