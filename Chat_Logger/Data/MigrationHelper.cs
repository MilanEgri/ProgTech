using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Logger.Data
{
    public static class MigrationHelper
    {
        public static void ApplyMigrations()
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();

                // 1. verzió: Alap táblák
                ExecuteMigration(connection, 1, new[]
                {
                    // Táblák létrehozása külön parancsokban
                    @"CREATE TABLE IF NOT EXISTS Messages (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Content TEXT NOT NULL,
                        Timestamp DATETIME NOT NULL
                    )",

                    @"CREATE TABLE IF NOT EXISTS Logs (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Timestamp DATETIME NOT NULL,
                        Level TEXT NOT NULL,
                        Message TEXT NOT NULL
                    )",
                    
                    // Beszúrás csak a táblák létrejötte után
                    @"INSERT INTO Logs (Timestamp, Level, Message) 
                      VALUES (datetime('now'), 'Info', 'Adatbázis migrálva verzió 1-re')"
                });

                // További migrációk...
            }
        }

        private static void ExecuteMigration(SQLiteConnection connection, int version, string[] sqlCommands)
        {
            // Ellenőrizzük, hogy a migráció már megtörtént-e
            using (var checkCmd = new SQLiteCommand(
                "SELECT COUNT(*) FROM Logs WHERE Message LIKE 'Adatbázis migrálva verzió @Version-re'",
                connection))
            {
                checkCmd.Parameters.AddWithValue("@Version", version);
                var count = (long)checkCmd.ExecuteScalar();

                if (count > 0) return; // Migráció már megtörtént
            }

            // Migrációs szkriptek végrehajtása tranzakcióban
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var sql in sqlCommands)
                    {
                        using (var command = new SQLiteCommand(sql, connection, transaction))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private static string GetConnectionString()
        {
            var dbPath = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "ChatLogger.db");

            return $"Data Source={dbPath};Version=3;";
        }
    }
}
