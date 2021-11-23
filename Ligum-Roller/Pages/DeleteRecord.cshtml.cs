using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Ligum_Roller.Pages
{
    public class DeleteRecordModel : PageModel
    {
        private readonly ILogger _logger;

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Barcode { get; set; }

        public DeleteRecordModel(ILogger<DeleteRecordModel> logger)
		{
            _logger = logger;
		}

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
            _logger.LogInformation("Deleted record {id}", Id);
            return RedirectToPage("./Index");
        }
    }
}
