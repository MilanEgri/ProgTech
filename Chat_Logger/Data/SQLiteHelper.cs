using Chat_Logger.Domain;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Logger.Data
{
    public static class SQLiteHelper
    {
        private static readonly string ConnectionString = SQLiteDatabaseManager.ConnectionString;

        public static void AddMessage(ChatMessage message)
        {
            ExecuteNonQuery(
                "INSERT INTO Messages (Content, Timestamp) VALUES (@content, @timestamp)",
                new SQLiteParameter("@content", message.Content),
                new SQLiteParameter("@timestamp", message.Timestamp));
        }

        public static List<ChatMessage> GetAllMessages()
        {
            return ExecuteQuery("SELECT * FROM Messages ORDER BY Timestamp DESC", reader =>
                new ChatMessage
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Content = reader["Content"].ToString(),
                    Timestamp = Convert.ToDateTime(reader["Timestamp"])
                });
        }

        public static void AddLog(LogEntry log)
        {
            ExecuteNonQuery(
                "INSERT INTO Logs (Timestamp, Level, Message) VALUES (@timestamp, @level, @message)",
                new SQLiteParameter("@timestamp", log.Timestamp),
                new SQLiteParameter("@level", log.Level),
                new SQLiteParameter("@message", log.Message));
        }

        public static List<LogEntry> GetAllLogs()
        {
            return ExecuteQuery("SELECT * FROM Logs ORDER BY Timestamp DESC", reader =>
                new LogEntry
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Timestamp = Convert.ToDateTime(reader["Timestamp"]),
                    Level = reader["Level"].ToString(),
                    Message = reader["Message"].ToString()
                });
        }

        private static void ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        private static List<T> ExecuteQuery<T>(string sql, Func<SQLiteDataReader, T> mapper)
        {
            var results = new List<T>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(mapper(reader));
                    }
                }
            }

            return results;
        }
    }
}
