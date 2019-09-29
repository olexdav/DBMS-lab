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
            RefreshRowsDataGridView();
        }

        private void editRowButton_Click(object sender, EventArgs e)
        {
            DBRow new_row = table.InputRow();
            int selected_row_index = rowsDataGridView.SelectedRows[0].Index;
            table.ReplaceRow(new_row, selected_row_index);
            RefreshRowsDataGridView();
        }

        private void deleteRowButton_Click(object sender, EventArgs e)
        {
            table.DeleteRow(rowsDataGridView.SelectedRows[0].Index);
            RefreshRowsDataGridView();
        }

        private void addRowButton_Click(object sender, EventArgs e)
        {
            DBRow new_row = table.InputRow();
            table.AddRow(new_row);
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
            rowsDataGridView.Rows.Clear();
            List<List<string>> text_repr = table.GetTextRepresentation();
            int row_index = 0;
            foreach (List<string> row in text_repr)
            {
                rowsDataGridView.Rows.Add();
                for (int x = 0; x < row.Count; x++)
                {
                    rowsDataGridView[x, row_index].Value = row[x];
                }
                row_index = row_index + 1;
            }
        }

        private void rowsDataGridView_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            // Set all newly added columns as unsortable
            rowsDataGridView.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm(table);
            searchForm.ShowDialog();
        }
    }
}
