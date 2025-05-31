using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Logger.Data
{
    public static class SQLiteDatabaseManager
    {
        private static readonly string DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ChatLogger.db");

        public static readonly string ConnectionString = $"Data Source={DbPath};Version=3;";

        public static void InitializeDatabase()
        {
            try
            {
                //Ell, letezik e az adatbazis

                bool isNewDatabase = !File.Exists(DbPath);

                //Ha nem akkor letrehozzuk
                if (isNewDatabase)
                {
                    SQLiteConnection.CreateFile(DbPath);
                }

                //Creating 2 tables

                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    //Messages table

                    ExecuteNonQuery(connection, @"
                        CREATE TABLE IF NOT EXISTS Messages (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Content TEXT NOT NULL,
                            Timestamp DATETIME NOT NULL
                        )");

                    //Logs table

                    ExecuteNonQuery(connection, @"
                        CREATE TABLE IF NOT EXISTS Logs (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Message TEXT NOT NULL,
                            Level TEXT NOT NULL,
                            Timestamp DATETIME NOT NULL
                        )");

                    //Startup postmessage (new database)

                    if (isNewDatabase)
                    {
                        ExecuteNonQuery(connection, @"INESRT INTO Logs(Timestamp, Level, Message) VALUES (datetime('now'), 'Info', 'Atabázis létrehozva')");
                    }
                }
            }
            catch (Exception e)
            {

                throw new ApplicationException("Adatbázis inicializálási hiba: " + e.Message, e);
            }
        }

        public static bool TestDatabaseConnection()
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand("SELECT 1 FROM Logs LIMIT 1", connection))
                    {
                        command.ExecuteScalar();
                    }
                    return true;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show($"Adatbázis hiba: {e.Message}\n\n" +
                                "Az alkalmazás memóriában fog tárolni!",
                                "Adatbázis hiba",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return false;
            }
        }

        private static void ExecuteNonQuery(SQLiteConnection connection, string sql)
        {
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
