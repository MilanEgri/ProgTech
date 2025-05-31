using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Logger.Logging
{
    /// <summary>
    /// Logolási szintek
    /// </summary>
    //public enum LogLevel { Info, Warning, Error }

    /// <summary>
    /// Logolási műveletek interfésze (SOLID - Interface Segregation)
    /// </summary>
    public interface ILogger
    {
        void Log(string message, LogLevel level);
    }
}
