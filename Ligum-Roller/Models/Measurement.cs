using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ligum_Roller.Models
{
	public class Measurement
	{
		[Name("vzdialenost")]
		public double Distance { get; set; }
		[Name("priemer")]
		public double Diameter { get; set; }
	}
}
