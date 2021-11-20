using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ligum_Roller.Models
{
	public class PdfConfig
	{
		public string Title { get; set; }
		public string CompanyName { get; set; }
		public string[] Property1 { get; set; } = new string[2];
		public string[] Property2 { get; set; } = new string[2];
		public string[] Property3 { get; set; } = new string[2];
		public string[] Property4 { get; set; } = new string[2];
		public string[] Property5 { get; set; } = new string[2];

	}
}
