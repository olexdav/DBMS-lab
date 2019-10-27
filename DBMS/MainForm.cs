using System;
using System.Windows.Forms;
using System.IO;

using MongoDB.Bson;
using MongoDB.Driver;

namespace DBMS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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
                DatabaseForm dbForm = new DatabaseForm(null, fileToOpen, "File");
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
                DatabaseForm dbForm = new DatabaseForm(newDBName, null, "New");
                this.Visible = false;
                dbForm.ShowDialog();
                this.Close();
            }
        }

        private void loadFromPGButton_Click(object sender, EventArgs e)
        {
            DatabaseForm dbForm = new DatabaseForm(null, null, "Postgres");
            this.Visible = false;
            dbForm.ShowDialog();
            this.Close();
        }

        private void loadFromMongoButton_Click(object sender, EventArgs e)
        {
            DatabaseForm dbForm = new DatabaseForm(null, null, "MongoDB");
            this.Visible = false;
            dbForm.ShowDialog();
            this.Close();
        }
    }
}
