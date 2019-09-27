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
    public partial class TableForm : Form
    {
        DBTable table;

        public TableForm(DBTable _table)
        {
            InitializeComponent();
            table = _table;
            this.Text = table.GetName();
            deleteRowButton.Enabled = false;
            editRowButton.Enabled = false;

            // Load field names as column headers
            foreach (string colname in table.GetFieldList())
                rowsDataGridView.Columns.Add(colname, colname);
        }

        private void editRowButton_Click(object sender, EventArgs e)
        {
            RefreshRowsDataGridView();
        }

        private void deleteRowButton_Click(object sender, EventArgs e)
        {
            RefreshRowsDataGridView();
        }

        private void addRowButton_Click(object sender, EventArgs e)
        {
            DBRow new_row = table.InputRow();
            RefreshRowsDataGridView();
        }

        private void rowsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // Activate "Delete row" and "Edit row" buttons
            // only when a row is selected
            deleteRowButton.Enabled = (rowsDataGridView.SelectedRows.Count > 0);
            editRowButton.Enabled = (rowsDataGridView.SelectedRows.Count > 0);
        }

        private void RefreshRowsDataGridView()
        {
            // TODO
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm();
            searchForm.ShowDialog();
        }
    }
}
