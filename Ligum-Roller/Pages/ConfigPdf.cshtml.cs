using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ligum_Roller.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Ligum_Roller.Pages
{
    public class ConfigPdfModel : PageModel
    {
        private readonly ILogger _logger;

		public Roller Roller { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
		[BindProperty]
		public PdfConfig PdfConfig { get; set; }
		public bool GlobalConfig { get; set; }
		public bool Success { get; set; }

		public ConfigPdfModel(ILogger<ConfigPdfModel> logger)
		{
			_logger = logger;
		}

		public async Task<IActionResult> OnGet()
        {
			PdfConfig = await DataLayer.ReadPdfConfig();
			if (PdfConfig == null)
			{
				PdfConfig = new PdfConfig();
			}

			// for specific roller
			if (!string.IsNullOrEmpty(Id))
			{
				string data = await DataLayer.ReadRecord(Id);
				if (data == null)
				{
					return NotFound();
				}
				Roller = DataLayer.ParseCsv(data);
				if (Roller == null)
				{
					return StatusCode(500);
				}
				Roller.Timestamp = DataLayer.ParseDateTime(Id.Split("~")[0]);
				PdfConfig.Barcode = Roller.Barcode;
			}
			// global config
			else
			{
				GlobalConfig = true;
				Roller = new Roller()
				{
					Timestamp = DateTime.Now,
					Barcode = "èiarový kód"
				};
			}
			return Page();
		}

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
			{
				return StatusCode(400);
			}
			await DataLayer.SavePdfConfig(PdfConfig);
			GlobalConfig = true;
			Roller = new Roller()
			{
				Timestamp = DateTime.Now,
				Barcode = "èiarový kód"
			};

			_logger.LogInformation("Changed PDF protocol config");
			Success = true;
			return Page();
		}
	}
}
