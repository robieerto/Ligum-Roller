using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Ligum_Roller.Models;

namespace Ligum_Roller
{
	public static class DataLayer
	{
		static readonly string dateTimeFormat = "dd-MM-yyyy_HH-mm-ss";
		static readonly CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			PrepareHeaderForMatch = args => args.Header.ToLower(),
		};
		static readonly char sep = Path.DirectorySeparatorChar;
		static readonly string currDir = Directory.GetCurrentDirectory();
		static readonly string dataPath = $"{currDir}{sep}data{sep}";
		static readonly string csvPath = $"{dataPath}csv{sep}";
		static readonly string graphPath = $"{dataPath}graph{sep}";

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
				if (!Directory.Exists(csvPath))
				{
					Directory.CreateDirectory(csvPath);
				}
				await File.WriteAllTextAsync(csvPath + recordName + ".csv", data);
				CreateGraph(recordName);
			}
			catch (Exception) { }
		}

		public static async Task<string> ReadRecord(string recordName)
		{
			try
			{
				if (Directory.Exists(csvPath))
				{
					return await File.ReadAllTextAsync(csvPath + recordName + ".csv");
				}
			}
			catch (Exception) { }
			return null;
		}

		public static async Task<byte[]> ReadRecordBytes(string recordName)
		{
			try
			{
				if (Directory.Exists(csvPath))
				{
					return await File.ReadAllBytesAsync(csvPath + recordName + ".csv");
				}
			}
			catch (Exception) { }
			return null;
		}

		public static List<string> GetAllRecords()
		{
			try
			{
				return Directory.GetFiles(csvPath)
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
				File.Delete(csvPath + recordName + ".csv");
				File.Delete(graphPath + recordName + ".png");
			}
			catch (Exception) { }
		}

		public static void RemoveAllRecords()
		{
			foreach (var record in GetAllRecords())
			{
				try
				{
					RemoveRecord(record);
				}
				catch (Exception) { }
			}
		}

		public static void CreateGraph(string recordName)
		{
			RunCmd.Run("graphCmd.py", csvPath + recordName + ".csv");
		}

		public static async Task SavePdfConfig(PdfConfig pdfConfig)
		{
			try
			{
				if (!Directory.Exists(dataPath))
				{
					Directory.CreateDirectory(dataPath);
				}
				var jsonString = JsonSerializer.Serialize(pdfConfig);
				await File.WriteAllTextAsync(dataPath + "pdfConfig.json", jsonString);
			}
			catch (Exception) { }
		}

		public static async Task<PdfConfig> ReadPdfConfig()
		{
			try
			{
				if (Directory.Exists(csvPath))
				{
					var jsonString = await File.ReadAllTextAsync(dataPath + "pdfConfig.json");
					return JsonSerializer.Deserialize<PdfConfig>(jsonString);
				}
			}
			catch (Exception) { }
			return new PdfConfig();
		}
	}
}
