using Chat_Logger.Data;
using Chat_Logger.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Logger.Logging
{
    public class DatabaseLogHandler : LogHandler
    {
        public DatabaseLogHandler(LogHandler next) : base(next) { }

        protected override bool CanHandle(LogLevel level)
        {
            // Minden szintet kezelünk
            return true;
        }

        protected override void Write(LogLevel level, string message)
        {
            try
            {
                using (var context = new ChatDbContext())
                {
                    context.Logs.Add(new LogEntry
                    {
                        Timestamp = DateTime.Now,
                        Level = level.ToString(),
                        Message = message
                    });
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Hibák a konzolra
                Console.WriteLine($"Hiba a log írása közben: {ex.Message}");
            }
        }
    }
}
