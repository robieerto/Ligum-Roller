using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ligum_Roller.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ligum_Roller.Pages
{
    public class IndexModel : PageModel
    {
        public IList<string> Records { get; set; }

        public void OnGet()
        {
            Records = DataLayer.GetAllRecords();
        }
    }
}
