using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DBMS
{
    public partial class DatabaseForm : Form
    {
        Database db;

        public DatabaseForm()
        {
            InitForm();
        }

        public DatabaseForm(string dbName, string dbPath, string source)
        {
            InitForm();
            if (source.Equals("MongoDB"))
            { // Load DB from MongoDB
                db = new Database();
                db.LoadFromMongo();
            }
            else if (source.Equals("Postgres"))
            { // Load DB from PostgreSQL
                db = new Database();
                db.LoadFromPostgres();
            }
            else if (!String.IsNullOrEmpty(dbPath))
            { // Load DB from file
                db = new Database();
                db.LoadFromJSON(dbPath);
            }
            else // Create new db
                db = new Database(dbName);
            RefreshDBTablesListBox();
        }

        private void InitForm()
        {
            InitializeComponent();
            viewTableButton.Enabled = false;
            deleteTableButton.Enabled = false;
            saveDBButton.Enabled = false;
        }

        private void DatabaseForm_Load(object sender, EventArgs e)
        {
            this.Text = db.GetName();
        }

        private void viewTableButton_Click(object sender, EventArgs e)
        {
            ViewSelectedTable();
        }

        private void dbTablesListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dbTablesListBox.SelectedIndex != -1)
                ViewSelectedTable();
        }

        private void ViewSelectedTable()
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
            if (!String.IsNullOrWhiteSpace(newTableName))
            {
                DBTable newTable = new DBTable(newTableName);
                TableFieldForm dbForm = new TableFieldForm(newTableName, newTable);
                dbForm.ShowDialog();
                if (dbForm.DialogResult == DialogResult.OK)
                {
                    db.AddTable(newTable);
                    RefreshDBTablesListBox();
                }
            }
        }

        private void saveDBButton_Click(object sender, EventArgs e)
        {
            string fileName = db.GetSaveFilename();
            if (!String.IsNullOrEmpty(fileName)) // Save DB to the last known file
            {
                db.SaveToJSON(fileName);
                MessageBox.Show("Database saved successfully!");
            }
            else
            { // Save DB to a new file
                fileName = Microsoft.VisualBasic.Interaction.InputBox("Enter name to save your database:",
                                                                      "Save database",
                                                                      db.GetName()+".txt");
                if (!String.IsNullOrWhiteSpace(fileName))
                    db.SaveToJSON("../../databases/" + fileName);
            }
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

        private void joinTablesButton_Click(object sender, EventArgs e)
        {
            // Choose first table
            string ltable = null;
            while (String.IsNullOrWhiteSpace(ltable))
                ltable = Microsoft.VisualBasic.Interaction.InputBox("Name of the first table?",
                                                                    "Choose first table", "");
            // Choose field to join on
            string lfield = null;
            while (String.IsNullOrWhiteSpace(lfield))
                lfield = Microsoft.VisualBasic.Interaction.InputBox("Field to join on?",
                                                                    "Choose field", "");
            // Choose second table
            string rtable = null;
            while (String.IsNullOrWhiteSpace(rtable))
                rtable = Microsoft.VisualBasic.Interaction.InputBox("Name of the second table?",
                                                                    "Choose second table", "");
            // Choose field in second table
            string rfield = null;
            while (String.IsNullOrWhiteSpace(rfield))
                rfield = Microsoft.VisualBasic.Interaction.InputBox("Field to join on?",
                                                                    "Choose field", "");
            // Join 2 tables and save as a separate table
            db.JoinTables(ltable, lfield, rtable, rfield);
            RefreshDBTablesListBox(); // Refresh view
        }

        private void saveToPGButton_Click(object sender, EventArgs e)
        {
            db.SaveToPostgres();
        }

        private void saveToMongoButton_Click(object sender, EventArgs e)
        {
            db.SaveToMongo();
        }
    }
}
