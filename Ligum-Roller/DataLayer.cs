using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Ligum_Roller.Models;

namespace Ligum_Roller
{
	public static class DataLayer
	{
		public static readonly string dateTimeFormat = "dd-MM-yyyy_HH-mm-ss";
		public static readonly string dataDir = "csvData" + Path.DirectorySeparatorChar;
		public static readonly string path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + dataDir;
		static readonly CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			PrepareHeaderForMatch = args => args.Header.ToLower(),
		};

		public static string GetCurrentDateTimeStr()
		{
			return DateTime.Now.ToString(dateTimeFormat);
		}

		public static DateTime? ParseDateTime(string dateTime)
		{
			try
			{
				return DateTime.ParseExact(dateTime, dateTimeFormat, null);

			}
			catch (Exception)
			{
				return null;
			}
		}

		public static Roller ParseCsv(string csv)
		{
			try
			{
				using var strReader = new StringReader(csv);
				using var csvReader = new CsvReader(strReader, csvConfig);
				var measurements = csvReader.GetRecords<Measurement>().ToList();
				string barcode = ParseCsvBarcode(csv);
				var roller = new Roller()
				{
					Measurements = measurements,
					Barcode = barcode
				};
				return roller;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static string ParseCsvBarcode(string csv)
		{
			try
			{
				using var strReader = new StringReader(csv);
				using var csvReader = new CsvReader(strReader, csvConfig);
				csvReader.Read();
				string barcode = csvReader.GetRecord(new { ciarovy_kod = "" }).ciarovy_kod;
				return barcode;
			}
			catch (Exception)
			{
				return "";
			}
		}

		public static async Task SaveRecord(string data, string recordName)
		{
			try
			{
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				await File.WriteAllTextAsync(path + recordName + ".csv", data);
			}
			catch (Exception)
			{
			}
		}

		public static async Task<string> ReadRecord(string recordName)
		{
			try
			{
				if (Directory.Exists(path))
				{
					return await File.ReadAllTextAsync(path + recordName + ".csv");
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		public static async Task<byte[]> ReadRecordBytes(string recordName)
		{
			try
			{
				if (Directory.Exists(path))
				{
					return await File.ReadAllBytesAsync(path + recordName + ".csv");
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		public static List<string> GetAllRecords()
		{
			try
			{
				return Directory.GetFiles(path)
					.OrderByDescending(f => new FileInfo(f).CreationTime)
					.Select(Path.GetFileNameWithoutExtension)
					.ToList();
			}
			catch (Exception)
			{
				return new List<string>();
			}
		}

		public static void RemoveRecord(string recordName)
		{
			try
			{
				File.Delete(path + recordName + ".csv");
			}
			catch (Exception)
			{
			}
		}

		public static void RemoveAllRecords()
		{
			try
			{
				foreach (var record in GetAllRecords())
				{
					RemoveRecord(record);
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
