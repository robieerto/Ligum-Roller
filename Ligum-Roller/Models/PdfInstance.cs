using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ligum_Roller.Models
{
	public class PdfInstance
	{
		public string Id { get; set; }
		public PdfConfig PdfConfig { get; set; }
		public Roller Roller { get; set; }
	}
}
