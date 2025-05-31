using Chat_Logger.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Logger
{
    public partial class LogViewerForm : Form
    {
        public LogViewerForm()
        {
            InitializeComponent();
            LoadLogs();
        }

        private void LoadLogs()
        {
            try
            {
                using (var context = new ChatDbContext())
                {
                    // Logbejegyzések betöltése időrendben
                    var logs = context.Logs
                        .OrderByDescending(l => l.Timestamp)
                        .ToList();

                    // DataGridView adatforrás beállítása
                    dgvLogs.DataSource = logs;

                    // Oszlopfejlécek beállítása
                    dgvLogs.Columns["Id"].Visible = false;
                    dgvLogs.Columns["Timestamp"].HeaderText = "Időpont";
                    dgvLogs.Columns["Level"].HeaderText = "Szint";
                    dgvLogs.Columns["Message"].HeaderText = "Üzenet";

                    // Oszlopszélességek beállítása
                    dgvLogs.Columns["Timestamp"].Width = 150;
                    dgvLogs.Columns["Level"].Width = 80;
                    dgvLogs.Columns["Message"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a naplók betöltésekor: {ex.Message}",
                                "Hiba",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLogs();
        }
    }
}
