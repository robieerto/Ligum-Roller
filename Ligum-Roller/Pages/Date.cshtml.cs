using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ligum_Roller.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Ligum_Roller.Pages
{
    public class DateModel : PageModel
    {
        public IList<string> Records { get; set; }
        [BindProperty]
        public IFormFile FileUpload { get; set; }
        public string Id { get; set; }
        private readonly ILogger _logger;

        public DateModel(ILogger<DateModel> logger)
		{
            _logger = logger;
        }

        public void OnGet(string id)
        {
            Id = id;
            if (!string.IsNullOrEmpty(Id))
			{
                Records = DataLayer.GetRecordsByDate(Id);
			}
            else
			{
                Records = DataLayer.GetAllRecords();
            }
        }

        public async Task<IActionResult> OnPostUpload()
        {
            _logger.LogInformation("Data file uploaded manually");
            var timestamp = DataLayer.GetCurrentDateTimeStr();
            using (var memoryStream = new MemoryStream())
            {
                await FileUpload.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    using var reader = new StreamReader(memoryStream);
                    var stringData = await reader.ReadToEndAsync();
                    var roller = DataLayer.ParseCsv(stringData);
                    if (roller != null)
					{
                        var id = timestamp + "~" + roller.Barcode;
                        await DataLayer.SaveRecord(stringData, id, _logger);
                    }
                    else
					{
                        _logger.LogError("Wrong format of data");
                    }
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
