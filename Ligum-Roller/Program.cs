using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ligum_Roller.FileLogger;

namespace Ligum_Roller
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			Directory.SetCurrentDirectory(AppContext.BaseDirectory);
			Directory.CreateDirectory(DataLayer.csvPath);
			Directory.CreateDirectory(DataLayer.graphPath);
			await CreateHostBuilder(args).Build().RunAsync();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseWindowsService()
				.ConfigureLogging((hostBuilderContext, logging) =>
				{
					logging.AddFileLogger(options =>
					{
						hostBuilderContext.Configuration.GetSection("Logging").GetSection("LogFile").GetSection("Options").Bind(options);
					});
				})
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.CaptureStartupErrors(true);
					webBuilder.UseSetting("detailedErrors", "true");
					webBuilder.UseKestrel(kestrelOptions =>
					{
						kestrelOptions.ConfigureHttpsDefaults(httpsOptions =>
						{
							httpsOptions.SslProtocols = SslProtocols.None;
						});
					});

					webBuilder.UseStartup<Startup>();
				});
	}
}
