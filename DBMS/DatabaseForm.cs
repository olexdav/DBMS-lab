using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//using DBMS

namespace DBMS
{
    public partial class DatabaseForm : Form
    {
        Database db;

        public DatabaseForm(String dbName)
        {
            InitializeComponent();
            this.Text = dbName;
            db = new Database(dbName);
            viewTableButton.Enabled = false;
            deleteTableButton.Enabled = false;
            saveDBButton.Enabled = false;
        }

        private void viewTableButton_Click(object sender, EventArgs e)
        {
            DBTable selectedTable = db.GetTable(dbTablesListBox.SelectedIndex);
            TableForm tableForm = new TableForm(selectedTable);
            tableForm.ShowDialog();
        }

        private void deleteTableButton_Click(object sender, EventArgs e)
        {
            db.DeleteTable(dbTablesListBox.SelectedIndex);
            RefreshDBTablesListBox();
            deleteTableButton.Enabled = false;
        }

        private void addTableButton_Click(object sender, EventArgs e)
        {
            string newTableName = Microsoft.VisualBasic.Interaction.InputBox("Enter name for a new table:",
                                                                             "New table", "Nice-Table");
            DBTable newTable = new DBTable(newTableName);
            TableFieldForm dbForm = new TableFieldForm(newTableName, newTable);
            dbForm.ShowDialog();
            if (dbForm.DialogResult == DialogResult.OK)
            {
                db.AddTable(newTable);
                RefreshDBTablesListBox();
            }
        }

        private void saveDBButton_Click(object sender, EventArgs e)
        {
            db.SaveToJSON("../../databases/kittens.txt");
        }

        private void RefreshDBTablesListBox()
        {
            dbTablesListBox.Items.Clear();
            List<string> tableDescList = db.GetTableDescList();
            foreach (string tableDesc in tableDescList)
            {
                dbTablesListBox.Items.Add(tableDesc);
            }
            // Only enable "Save Database" button if there is at least one table
            saveDBButton.Enabled = (tableDescList.Count > 0);
        }

        private void dbTablesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Activate "Delete table" and "View table" buttons only if a table is selected
            deleteTableButton.Enabled = (dbTablesListBox.SelectedIndex != -1);
            viewTableButton.Enabled = (dbTablesListBox.SelectedIndex != -1);
        }
    }
}
