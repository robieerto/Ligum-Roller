using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ligum_Roller.Models
{
	public class PdfConfig
	{
		[Required(ErrorMessage = "Hodnota je povinná.")]
		[Range(3, 5000, ErrorMessage = "Hodnota musí byť nepárna v rozmedzí 3 až 5000.")]
		[RegularExpression(@"^\d*[13579]$", ErrorMessage = "Hodnota musí byť nepárna v rozmedzí 3 až 5000.")]
		public int NumValues { get; set; }
		[MaxLength(100)]
		public string Title { get; set; }
		[MaxLength(100)]
		public string CompanyName { get; set; }
		public PdfConfigValue Property1 { get; set; }
		public PdfConfigValue Property2 { get; set; }
		public PdfConfigValue Property3 { get; set; }
		public PdfConfigValue Property4 { get; set; }
		public PdfConfigValue Property5 { get; set; }

	}
}
