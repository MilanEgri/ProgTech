using Chat_Logger.Data;
using Chat_Logger.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Logger.Logging
{
    public class DatabaseLogger : ILogger
    {
        public void Log(string message, LogLevel level)
        {
            SQLiteHelper.AddLog(new LogEntry
            {
                Timestamp = DateTime.Now,
                Level = level.ToString(),
                Message = message
            });
        }
    }
}
