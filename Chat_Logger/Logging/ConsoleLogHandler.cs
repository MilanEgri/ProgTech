using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Logger.Logging
{
    internal class ConsoleLogHandler : LogHandler
    {
        private readonly LogLevel _minLevel;


        public ConsoleLogHandler(LogLevel minLevel, LogHandler next) : base(next)
        {
            _minLevel = minLevel;
        }

        protected override bool CanHandle(LogLevel level)
        {
            return level >= _minLevel;
        }

        protected override void Write(LogLevel level, string message)
        {
            var stamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Console.WriteLine($"{stamp} [{level}] {message}");
        }
    }
}
