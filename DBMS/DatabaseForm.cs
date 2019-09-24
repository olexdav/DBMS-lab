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
        }

        private void viewTableButton_Click(object sender, EventArgs e)
        {

        }

        private void deleteTableButton_Click(object sender, EventArgs e)
        {

        }

        private void addTableButton_Click(object sender, EventArgs e)
        {
            String newTableName = Microsoft.VisualBasic.Interaction.InputBox("Enter name for a new table:",
                                                                             "New table", "NiceTable");
            DBTable newTable = new DBTable(newTableName);
            TableFieldForm dbForm = new TableFieldForm(newTableName, newTable);
            dbForm.ShowDialog();
            db.AddTable(newTable);
        }

        private void saveDBButton_Click(object sender, EventArgs e)
        {

        }
    }
}
