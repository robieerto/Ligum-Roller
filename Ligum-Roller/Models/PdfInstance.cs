using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ligum_Roller.Models
{
	public class PdfInstance
	{
		private const double _offsetDist = 50.0;

		public string Id { get; set; }
		public PdfConfig PdfConfig { get; set; }
		public Roller Roller { get; set; }
		public IList<Measurement> PdfMeasurements { get; set; }

		public PdfInstance(string id, PdfConfig pdfConfig, Roller roller)
		{
			Id = id;
			PdfConfig = pdfConfig;
			Roller = roller;
			SetPdfMeasurements();
		}

		private void SetPdfMeasurements()
		{
			var lastDist = Roller.Measurements.Last().Distance - _offsetDist;
			var allValues = Roller.Measurements
				.Where(m => (m.Distance >= _offsetDist) && (m.Distance <= lastDist))
				.ToList();
			var reducedIdxs = GetReducedIdxs(allValues);
			// if we have less values than it would be reduced ones
			if (reducedIdxs == null)
			{
				PdfMeasurements = allValues;
			}
			else {
				PdfMeasurements = new List<Measurement>();
				foreach (int idx in reducedIdxs)
				{
					PdfMeasurements.Add(allValues[idx]);
				}
			}
		}

		private int[] GetReducedIdxs(List<Measurement> values)
		{
			var count = values.Count(); // all values
			int numVals = PdfConfig.NumValues; // reduced values
			if (count <= numVals)
			{
				return null;
			}

			// pole indexov
			int[] idxs = new int[numVals];

			// stredny index vo vsetkych prvkoch
			int middleIdx = count / 2;
			// pocet vyberanych prvkov
			int halfValsCount = (numVals - 1) / 2;
			// krok vyberu hodnot
			int step = middleIdx / halfValsCount;
			int i;
			// prvy pred stredom
			for (i = 0; i < halfValsCount; ++i)
			{
				idxs[i] = step * i;
			}
			// stred
			idxs[i] = middleIdx;
			// prvy za stredom
			for (i = 0; i < halfValsCount; ++i)
			{
				idxs[numVals-1 - i] = count-1 - (step * i);
			}

			return idxs;
		}
	}
}
