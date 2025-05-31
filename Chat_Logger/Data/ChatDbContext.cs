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
    public class ChatDbContext : DbContext
    {
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<LogEntry> Logs { get; set; }

        public ChatDbContext() : base(new SQLiteConnection(GetConnectionString()), true)
        {
            // Adatbázis létrehozása, ha nem létezik
            if (!Database.Exists())
            {
                Database.Create();
                SeedDatabase();
            }
        }

        private static string GetConnectionString()
        {
            var dbPath = Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "ChatLogger.db");

            return $"Data Source={dbPath};Version=3;";
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // SQLite konfiguráció
            Database.SetInitializer(new SQLiteInitializer());
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }

        private void SeedDatabase()
        {
            // Kezdeti adatok
            Logs.Add(new LogEntry
            {
                Timestamp = System.DateTime.Now,
                Level = "Info",
                Message = "Adatbázis inicializálva"
            });
            SaveChanges();
        }

        private class SQLiteInitializer : IDatabaseInitializer<ChatDbContext>
        {
            public void InitializeDatabase(ChatDbContext context)
            {
                // Manuális táblalétrehozás
                CreateTableIfNotExists(context, "Messages", @"
                    CREATE TABLE Messages (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Content TEXT NOT NULL,
                        Timestamp DATETIME NOT NULL
                    )");

                CreateTableIfNotExists(context, "Logs", @"
                    CREATE TABLE Logs (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Timestamp DATETIME NOT NULL,
                        Level TEXT NOT NULL,
                        Message TEXT NOT NULL
                    )");
            }

            private void CreateTableIfNotExists(ChatDbContext context, string tableName, string createSql)
            {
                try
                {
                    // Tábla létezésének ellenőrzése
                    context.Database.ExecuteSqlCommand(
                        $"SELECT 1 FROM {tableName} LIMIT 1");
                }
                catch
                {
                    // Tábla nem létezik, létrehozzuk
                    context.Database.ExecuteSqlCommand(createSql);

                    // Naplózzuk a létrehozást
                    context.Logs.Add(new LogEntry
                    {
                        Timestamp = DateTime.Now,
                        Level = "Info",
                        Message = $"{tableName} tábla létrehozva"
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
