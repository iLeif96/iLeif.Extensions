using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLeif.Extensions.Logging
{
    public class TextWriterLogger : ILogger
    {
        private ITextWriter? _writer;

        public TextWriterLogger(ITextWriter? writer)
        {
            _writer = writer;
        }

        public void SetWriter(ITextWriter writer)
        {
            _writer = writer;
        }

        public void Log(string message)
        {
            _writer?.WriteLine(message);
        }

        public void Info(string message)
        {
            _writer?.WriteLine("INFO: " + message);
        }

        public void Warn(string message)
        {
            _writer?.WriteLine("WARN: " + message);
        }

        public void Error(string message)
        {
            _writer?.WriteLine("ERROR: " + message);
        }
    }
}
