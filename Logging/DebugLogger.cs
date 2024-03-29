﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLeif.Extensions.Logging
{
    public class DebugLogger : ILogger
    {
        public DebugLogger() { }

        public void Log(string message)
        {
            Debug.WriteLine(message);
        }

        public void Info(string message)
        {
            Debug.WriteLine("INFO: " + message);
        }

        public void Warn(string message)
        {
            Debug.WriteLine("WARN: " + message);
        }

        public void Error(string message)
        {
            Debug.WriteLine("ERROR: " + message);
        }
    }

	public class ConsoleLogger : ILogger
	{
		public ConsoleLogger() { }

		public void Log(string message)
		{
			Console.WriteLine(message);
		}

		public void Info(string message)
		{
			Console.WriteLine("INFO: " + message);
		}

		public void Warn(string message)
		{
			Console.WriteLine("WARN: " + message);
		}

		public void Error(string message)
		{
			Console.WriteLine("ERROR: " + message);
		}
	}
}
