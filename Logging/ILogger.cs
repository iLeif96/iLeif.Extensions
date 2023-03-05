using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLeif.Extensions.Logging
{
    public interface ILogger
    {
        public void Log(string message);

        public void Info(string message);

        public void Warn(string message);

        public void Error(string message);
    }
}
