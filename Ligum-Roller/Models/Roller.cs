using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ligum_Roller.Models
{
	public class Roller
	{
		public DateTime? Timestamp { get; set; }
		public string Barcode { get; set; }
		public IEnumerable<Measurement> Measurements { get; set; }
	}
}
