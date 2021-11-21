using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ligum_Roller.Models
{
	public class PdfConfigValue
	{
		[MaxLength(100)]
		public string Name { get; set; }
		[MaxLength(100)]
		public string Value { get; set; }
	}
}
