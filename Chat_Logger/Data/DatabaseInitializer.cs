using Chat_Logger.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Logger.Data
{
    public class DatabaseInitializer
    {
        public static void Initialize()
        {
            var dbPath = GetDatabasePath();

            // Ha nem létezik az adatbázis fájl, létrehozzuk
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                CreateTables(connection);
            }
        }

        private static string GetDatabasePath()
        {
            return Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "ChatLogger.db");
        }

        private static void CreateTables(SQLiteConnection connection)
        {
            // Messages tábla létrehozása (ha nem létezik)
            ExecuteSql(connection, @"
                CREATE TABLE IF NOT EXISTS Messages (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Content TEXT NOT NULL,
                    Timestamp DATETIME NOT NULL
                )");

            // Logs tábla létrehozása (ha nem létezik)
            ExecuteSql(connection, @"
                CREATE TABLE IF NOT EXISTS Logs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Timestamp DATETIME NOT NULL,
                    Level TEXT NOT NULL,
                    Message TEXT NOT NULL
                )");

            // Kezdeti bejegyzés hozzáadása
            ExecuteSql(connection, @"
                INSERT INTO Logs (Timestamp, Level, Message)
                SELECT datetime('now'), 'Info', 'Adatbázis inicializálva'
                WHERE NOT EXISTS (SELECT 1 FROM Logs)
            ");
        }

        private static void ExecuteSql(SQLiteConnection connection, string sql)
        {
            try
            {
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                throw new ApplicationException($"SQL hiba: {ex.Message}\nSQL: {sql}", ex);
            }
        }
    }
}
