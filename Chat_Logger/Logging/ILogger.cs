using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Logger.Logging
{
    
    /// Logolási műveletek interfésze (SOLID - Interface Segregation)
    public interface ILogger
    {
        void Log(string message, LogLevel level);
    }
}
