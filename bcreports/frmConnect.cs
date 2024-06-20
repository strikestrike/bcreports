using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bcreports
{
    public partial class frmConnect : Form
    {
        public static string connectString = "";
        public frmConnect()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ComboBoxAuthentication.DataSource = new string[] { "Windows Authentication", "SQL Server Authentication" };

            ComboBoxAuthentication.SelectedIndex = Properties.Settings.Default.connect_mode;
            TextBoxAddress.Text = Properties.Settings.Default.server_address ?? "localhost";
            TextBoxDBName.Text = Properties.Settings.Default.db_name ?? "BCP";
            TextBoxUserName.Text = Properties.Settings.Default.user_name;
            TextBoxPassword.Text = Properties.Settings.Default.user_pwd;
            CheckBoxEncrypt.Checked = Properties.Settings.Default.connect_encryption;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;

            Properties.Settings.Default.connect_mode = ComboBoxAuthentication.SelectedIndex;
            Properties.Settings.Default.server_address = TextBoxAddress.Text;
            Properties.Settings.Default.db_name = TextBoxDBName.Text;
            Properties.Settings.Default.user_name = TextBoxUserName.Text;
            Properties.Settings.Default.user_pwd = TextBoxPassword.Text;
            Properties.Settings.Default.connect_encryption = CheckBoxEncrypt.Checked;
            Properties.Settings.Default.Save();

            try
            {
                string userPassword = "";
                if (ComboBoxAuthentication.SelectedIndex == 1)
                {
                    userPassword = $";User ID={TextBoxUserName.Text};Password={TextBoxPassword.Text}";
                }

                string bEncrypt = CheckBoxEncrypt.Checked ? "True" : "False";

                connectString = $"Server={TextBoxAddress.Text};Initial Catalog={TextBoxDBName.Text};MultipleActiveResultSets=true;Integrated Security=True;Encrypt={bEncrypt}{userPassword}";
                SqlConnection connection = new SqlConnection
                {
                    ConnectionString = connectString
                };

                connection.Open();
                connection.Close();

                this.Hide();
                frmReport reportDialog = new frmReport();
                reportDialog.ShowDialog();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            btnConnect.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
