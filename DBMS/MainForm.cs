using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace DBMS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            //string kittenDBLink = "https://drive.google.com/uc?export=download&id=109GsRUBeYYndHjgX5fFEJ-bMm5YWegEW";
            //WebClient webClient = new WebClient();
            //string kittenData = System.Text.Encoding.UTF8.GetString(webClient.DownloadData(kittenDBLink));
            //Console.WriteLine(kittenData);
        }

        private void loadDBFromFileButton_Click(object sender, EventArgs e)
        {
            var FD = new System.Windows.Forms.OpenFileDialog();
            // Set default path
            string combinedPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                                                         "../../databases");
            FD.InitialDirectory = System.IO.Path.GetFullPath(combinedPath); ;
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileToOpen = FD.FileName;
                DatabaseForm dbForm = new DatabaseForm(null, fileToOpen);
                this.Visible = false;
                dbForm.ShowDialog();
                this.Close();
            }
        }

        /// <summary>
        /// Prompts user to enter DB name and opens a new database window
        /// </summary>
        private void createDBButton_Click(object sender, EventArgs e)
        {
            string newDBName = Microsoft.VisualBasic.Interaction.InputBox("How do you want to call the new database?",
                                                                          "Name your database", "Nice-DB");
            if (!String.IsNullOrWhiteSpace(newDBName))
            {
                DatabaseForm dbForm = new DatabaseForm(newDBName, null);
                this.Visible = false;
                dbForm.ShowDialog();
                this.Close();
            }
        }
    }
}
