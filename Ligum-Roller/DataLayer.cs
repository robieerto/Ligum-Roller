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
				var roller = new Roller();
				var config = new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					PrepareHeaderForMatch = args => args.Header.ToLower(),
				};
				var strReader = new StringReader(csv);
				var csvReader = new CsvReader(strReader, config);
				using (strReader) using (csvReader)
				{
					csvReader.Read();
					roller.Barcode = csvReader.GetRecord(new { ciarovy_kod = "" }).ciarovy_kod;
				}
				using (strReader = new StringReader(csv))
				using (csvReader = new CsvReader(strReader, config))
				{
					roller.Measurements = csvReader.GetRecords<Measurement>().ToList();
				}
				return roller;
			}
			catch (Exception)
			{
				return null;
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
