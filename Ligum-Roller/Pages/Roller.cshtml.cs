using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ligum_Roller.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ligum_Roller.Pages
{
	public class RollerModel : PageModel
	{
		public Roller Roller { get; set; }
		public ICollection<Measurement> Measurements { get; set; }
		[BindProperty(SupportsGet = true)]
		public string Id { get; set; }
		[BindProperty(SupportsGet = true)]
		public string SearchString { get; set; }

		public async Task<IActionResult> OnGet()
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
			Roller = DataLayer.ParseCsv(data);
			if (Roller == null)
			{
				return StatusCode(500);
			}
			Roller.Timestamp = DataLayer.ParseDateTime(Id);
			Measurements = Roller.Measurements;

			if (!string.IsNullOrEmpty(SearchString) && Measurements != null)
			{
				// comma separated decimal values
				var decNumRegex = @"[+-]?([0-9]*[.])?[0-9]+";
				var regex = String.Format(@"^({0})+(,*{0})*,*$", decNumRegex);
				SearchString = Regex.Replace(SearchString, @"\s", ""); // remove whitespaces
				if (Regex.Match(SearchString, regex).Success)
				{
					var searchNums = SearchString.Split(',')
						.Where(n => !string.IsNullOrEmpty(n))
						.Select(n => (int)double.Parse(n))
						.ToList();
					Measurements = Roller.Measurements
						.Where(m => searchNums.Exists(n => ((int)m.Distance).Equals(n)))
						.ToList();
				}
			}
			return Page();
		}

		public async Task<FileResult> OnGetDownloadCsv()
		{
			var dataBytes = await DataLayer.ReadRecordBytes(Id);
			return File(dataBytes, "application/octet-stream", Id + ".csv");
		}
	}
}
