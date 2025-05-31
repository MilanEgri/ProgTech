using Chat_Logger.Data;
using Chat_Logger.Logging;
using Chat_Logger.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Logger
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ILogger logger = new ChainLogger();
            IChatMessageRepository repository = null;

            try
            {
                // 1. Adatbázis inicializálása
                DatabaseInitializer.Initialize();

                // 2. Logger létrehozása
                logger = new ChainLogger();

                // 3. Naplózzuk az inicializálást
                logger.Log("Adatbázis inicializálás sikeres", LogLevel.Info);

                // 4. Adatbázis repository használata
                repository = new DatabaseChatRepository();

                // 5. Ellenőrizzük az adatbázis kapcsolatot
                TestDatabaseConnection(repository);
            }
            catch (Exception ex)
            {
                // Hibanaplózás
                if (logger == null)
                {
                    // Ha még a logger sem működik, konzolra írunk
                    Console.WriteLine($"Hiba: {ex.Message}");
                }
                else
                {
                    logger.Log($"Hiba az adatbázis inicializálásakor: {ex.Message}", LogLevel.Error);
                }

                // Felhasználói értesítés
                MessageBox.Show($"Adatbázis hiba: {ex.Message}\n\n" +
                                "Az alkalmazás memóriában fog tárolni!",
                                "Adatbázis hiba",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                // Memóriában tároló repository használata
                logger = logger ?? new ChainLogger();
                repository = new InMemoryChatRepository();
            }

            // Főablak indítása
            Application.Run(new MainForm(repository, logger));
        }

        private static void TestDatabaseConnection(IChatMessageRepository repository)
        {
            try
            {
                // Próba lekérés az adatbázisból
                var count = repository.GetAll().Count;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Adatbázis kapcsolat tesztelése sikertelen", ex);
            }
        }




    }
    
}
