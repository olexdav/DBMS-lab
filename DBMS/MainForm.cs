using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBMS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void loadDBFromFileButton_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Prompts user to enter DB name and opens a new database window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createDBButton_Click(object sender, EventArgs e)
        {
            String newDBName = Microsoft.VisualBasic.Interaction.InputBox("How do you want to call the new database?",
                                                                          "Name your database", "Nice-DB");
            DatabaseForm dbForm = new DatabaseForm(newDBName);
            this.Visible = false;
            dbForm.ShowDialog();
            this.Close();
        }
    }
}
