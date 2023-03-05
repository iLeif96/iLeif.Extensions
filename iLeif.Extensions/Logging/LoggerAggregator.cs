using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLeif.Extensions.Logging
{
    public class LoggerAggregator : ILogger
    {
        private readonly List<ILogger> _loggers = new List<ILogger>();

        public LoggerAggregator(params ILogger[] loggers)
        {
            AddLoggers(loggers);
        }

        public void AddLoggers(params ILogger[] loggers)
        {
            foreach (var logger in loggers)
            {
                _loggers?.Add(logger);
            }
        }

        public void Log(string message)
        {
            foreach (var logger in _loggers)
            {
                logger?.Log(message);
            }
        }

        public void Info(string message)
        {
            foreach (var logger in _loggers)
            {
                logger?.Info(message);
            }
        }

        public void Warn(string message)
        {
            foreach (var logger in _loggers)
            {
                logger?.Warn(message);
            }
        }

        public void Error(string message)
        {
            foreach (var logger in _loggers)
            {
                logger?.Error(message);
            }
        }
    }
}
