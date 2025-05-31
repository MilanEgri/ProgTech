using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Logger.Logging
{
    public enum LogLevel
    {
        Trace = 1,
        Debug,
        Info,
        Notice,
        Warn,
        Error,
        Critical,
        Fatal
    }
    // ==============================
    // CHAIN OF RESPONSIBILITY PATTERN
    // ==============================

    public abstract class LogHandler
    {
        private readonly LogHandler _next;

        protected LogHandler(LogHandler next) => _next = next;

        public void Handle(LogLevel level, string message)
        {
            if (CanHandle(level))
                Write(level, message);
            else
                _next?.Handle(level, message);
        }

        protected abstract bool CanHandle(LogLevel level);

        protected virtual void Write(LogLevel level, string message)
        {
            var stamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Console.WriteLine($"{stamp} [{level}] {message}");
        }
    }

    public sealed class LevelHandler : LogHandler
    {
        private readonly LogLevel _myLevel;

        public LevelHandler(LogLevel myLevel, LogHandler next) : base(next) => _myLevel = myLevel;

        protected override bool CanHandle(LogLevel level) => level == _myLevel;
    }

    public class ChainLogger : ILogger
    {
        private readonly LogHandler _rootHandler;

        public ChainLogger()
        {
            //// Lánc felépítése (Fatal -> Trace)
            //LogHandler current = null;
            //for (var lvl = LogLevel.Fatal; lvl >= LogLevel.Trace; lvl--)
            //    current = new LevelHandler(lvl, current);
            //_rootHandler = current;
            // Lánc felépítése (Fatal -> Trace)
            LogHandler current = null;

            // Adatbázis handler minden szinthez
            current = new DatabaseLogHandler(current);

            // Konzol handler csak magasabb szintekhez
            current = new ConsoleLogHandler(LogLevel.Warn, current);

            // További handler-ek...

            _rootHandler = current;
        }

        public void Log(string message, LogLevel level) => _rootHandler.Handle(level, message);
        
    }
}
