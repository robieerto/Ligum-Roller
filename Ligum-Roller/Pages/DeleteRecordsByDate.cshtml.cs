using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Ligum_Roller.Pages
{
    public class DeleteRecordsByDateModel : PageModel
    {
        private readonly ILogger _logger;

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public DeleteRecordsByDateModel(ILogger<DeleteRecordsByDateModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            DataLayer.RemoveRecordsByDate(Id);
            _logger.LogInformation("Deleted all records from date " + Id);
            return RedirectToPage("./Index");
        }
    }
}
