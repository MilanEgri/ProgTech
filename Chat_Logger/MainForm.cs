using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chat_Logger.Logging;
using Chat_Logger.Repository;
using Chat_Logger.Domain;

namespace Chat_Logger
{
    public partial class MainForm : Form
    {

        //// Függőségek injektálása (SOLID - Dependency Inversion)
        //private readonly IChatMessageRepository _repository;
        //private readonly ILogger _logger;

        //public MainForm(IChatMessageRepository repository, ILogger logger)
        //{
        //    InitializeComponent();

        //    // Függőségek tárolása
        //    _repository = repository;
        //    _logger = logger;

        //    // Eseménykezelők regisztrálása
        //    btnSend.Click += OnSendMessage;
        //    this.Load += OnFormLoad;
        //}

        //// Űrlap betöltésekor
        //private void OnFormLoad(object sender, EventArgs e)
        //{
        //    RefreshMessages();
        //    _logger.Log("Alkalmazás elindult", LogLevel.Info);
        //}

        //// Üzenet küldése gomb eseménykezelője
        //private void OnSendMessage(object sender, EventArgs e)
        //{
        //    //// Validáció: nem üzenet küldése
        //    //if (!string.IsNullOrWhiteSpace(txtMessage.Text))
        //    //{
        //    //    // Új üzenet objektum létrehozása
        //    //    var message = new ChatMessage(txtMessage.Text, DateTime.Now);

        //    //    // REPOSITORY PATTERN használata adattároláshoz
        //    //    _repository.Add(message);

        //    //    // Logolás
        //    //    _logger.Log($"Üzenet elküldve: {message.Content}", LogLevel.Info);

        //    //    // UI frissítése
        //    //    RefreshMessages();
        //    //    txtMessage.Clear();
        //    //}

        //    if (!string.IsNullOrWhiteSpace(txtMessage.Text))
        //    {
        //        var message = new ChatMessage(txtMessage.Text, DateTime.Now);
        //        _repository.Add(message);
        //        _logger.Log($"Üzenet elküldve: {message.Content}", LogLevel.Info);
        //        RefreshMessages();
        //        txtMessage.Clear();
        //    }
        //}

        //// Üzenetlista frissítése
        //private void RefreshMessages()
        //{
        //    // Üzenetek lekérése a repository-ból
        //    var messages = _repository.GetAll();

        //    // ListBox tartalmának frissítése
        //    lstMessages.DataSource = null;
        //    lstMessages.DataSource = messages;
        //    lstMessages.SelectedIndex = lstMessages.Items.Count - 1;
        //}

        private readonly IChatMessageRepository _repository;
        private readonly ILogger _logger;

        public MainForm(IChatMessageRepository repository, ILogger logger)
        {
            InitializeComponent();
            _repository = repository;
            _logger = logger;

            btnSend.Click += OnSendMessage;
            Load += OnFormLoad;
            btnViewLogs.Click += OnViewLogs; // Eseménykezelő hozzáadása
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            RefreshMessages();
            _logger.Log("Alkalmazás elindult", LogLevel.Info);
        }

        private void OnSendMessage(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                var message = new ChatMessage(txtMessage.Text, DateTime.Now);
                _repository.Add(message);
                _logger.Log($"Üzenet elküldve: {message.Content}", LogLevel.Info);
                RefreshMessages();
                txtMessage.Clear();
            }
        }

        private void RefreshMessages()
        {
            try
            {
                lstMessages.DataSource = null;
                lstMessages.DataSource = _repository.GetAll();
                lstMessages.DisplayMember = "Formatted"; // Formázott tartalom megjelenítése
            }
            catch (Exception ex)
            {
                _logger.Log($"Hiba az üzenetek frissítésekor: {ex.Message}", LogLevel.Error);
                MessageBox.Show($"Hiba az üzenetek betöltésekor: {ex.Message}");
            }
        }

        private void OnViewLogs(object sender, EventArgs e)
        {
            try
            {
                using (var logForm = new LogViewerForm())
                {
                    logForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Hiba a naplók megjelenítésekor: {ex.Message}", LogLevel.Error);
                MessageBox.Show($"Hiba a naplók megnyitása közben: {ex.Message}");
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            OnViewLogs(sender, e);
        }
    }
}
