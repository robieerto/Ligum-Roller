using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ligum_Roller.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ligum_Roller.Pages
{
    public class IndexModel : PageModel
    {
        public IList<string> Records { get; set; }
        [BindProperty]
        public IFormFile FileUpload { get; set; }

        public IndexModel()
		{
        }

        public void OnGet()
        {
            Records = DataLayer.GetAllRecords();
        }

        public async Task<IActionResult> OnPostUpload()
        {
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
                    if (DataLayer.ParseCsv(stringData) != null)
					{
                        await DataLayer.SaveRecord(stringData, timestamp);
                    }
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
