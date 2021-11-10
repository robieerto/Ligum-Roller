using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ligum_Roller.Pages
{
    public class DeleteRecordModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Barcode { get; set; }

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
            Barcode = DataLayer.ParseCsvBarcode(data);
            Timestamp = DataLayer.ParseDateTime(Id);
            return Page();
        }

        public IActionResult OnPost()
		{
            DataLayer.RemoveRecord(Id);
            return RedirectToPage("./Index");
        }
    }
}
