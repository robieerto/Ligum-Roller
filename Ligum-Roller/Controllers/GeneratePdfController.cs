using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ligum_Roller.Models;
using Wkhtmltopdf.NetCore;

namespace Ligum_Roller.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class GeneratePdfController : ControllerBase
	{
		readonly IGeneratePdf _generatePdf;
		[BindProperty(SupportsGet = true)]
		public string Id { get; set; }
		[BindProperty]
		public PdfConfig PdfConfig { get; set; }

		public GeneratePdfController(IGeneratePdf generatePdf)
		{
			_generatePdf = generatePdf;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			if (string.IsNullOrEmpty(Id))
			{
				return NotFound();
			}
			string data = await DataLayer.ReadRecord(Id);
			if (data == null)
			{
				return NotFound();
			}
			var model = new PdfInstance()
			{
				Id = Id,
				Roller = DataLayer.ParseCsv(data),
				PdfConfig = await DataLayer.ReadPdfConfig()
			};
			if (model.Roller == null)
			{
				return StatusCode(500);
			}
			model.Roller.Timestamp = DataLayer.ParseDateTime(Id);

			return await _generatePdf.GetPdf("Views/ProtocolPdf.cshtml", model);
		}

		[HttpPost]
		public async Task<IActionResult> Post()
		{
			if (string.IsNullOrEmpty(Id))
			{
				return NotFound();
			}
			string data = await DataLayer.ReadRecord(Id);
			if (data == null)
			{
				return NotFound();
			}
			var model = new PdfInstance()
			{
				Id = Id,
				Roller = DataLayer.ParseCsv(data),
				PdfConfig = await DataLayer.ReadPdfConfig()
			};
			if (model.Roller == null)
			{
				return StatusCode(500);
			}
			model.Roller.Timestamp = DataLayer.ParseDateTime(Id);
			model.PdfConfig = PdfConfig;

			return await _generatePdf.GetPdf("Views/ProtocolPdf.cshtml", model);
		}
	}
}
