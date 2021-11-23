using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Ligum_Roller.Pages
{
    public class DeleteAllRecordsModel : PageModel
    {
        private readonly ILogger _logger;

        public DeleteAllRecordsModel(ILogger<DeleteRecordModel> logger)
		{
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            DataLayer.RemoveAllRecords();
            _logger.LogInformation("Deleted all records");
            return RedirectToPage("./Index");
        }
    }
}
