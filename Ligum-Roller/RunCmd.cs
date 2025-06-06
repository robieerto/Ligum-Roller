﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ligum_Roller
{
    public static class RunCmd
    {
        public static string Run(string cmd, string args)
        {
			ProcessStartInfo start = new ProcessStartInfo
			{
				FileName = cmd,
				Arguments = string.Format("\"{0}\"", args),
				UseShellExecute = false,// Do not use OS shell
				CreateNoWindow = true, // We don't need new window
				RedirectStandardOutput = true,// Any output, generated by application will be redirected back
				RedirectStandardError = true // Any error in standard output will be redirected back (for example exceptions)
			};
			using Process process = Process.Start(start);
			using StreamReader reader = process.StandardOutput;
			string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
			string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
			return result;
		}
    }
}
