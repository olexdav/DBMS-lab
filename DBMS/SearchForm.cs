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
    public partial class SearchForm : Form
    {
        DBTable searchResultsTable;
        DBTable searchableTable;

        public SearchForm(DBTable table)
        {
            InitializeComponent();
            searchButton.Enabled = false;
            searchResultsTable = new DBTable();
            searchableTable = table;
            // Fill ComboBox options with supported types
            List<Element> supportedTypes = Element.GetSupportedTypes();
            foreach (Element el in supportedTypes)
                searchTypeComboBox.Items.Add(el.GetTypeName());
            // Select first type as a default option
            searchTypeComboBox.Text = searchTypeComboBox.Items[0].ToString();
            // Load field names as column headers
            foreach (string colname in table.GetFieldList())
                searchDataGridView.Columns.Add(colname, colname);
            RefreshSearchDataGridView();
        }

        private void FieldNameTextBox_TextChanged(object sender, EventArgs e)
        {
            searchButton.Enabled = (searchValueTextBox.Text.Length > 0);
        }

        private void RefreshSearchDataGridView()
        {
            searchDataGridView.Rows.Clear();
            List<List<string>> text_repr = searchResultsTable.GetTextRepresentation();
            int row_index = 0;
            foreach (List<string> row in text_repr)
            {
                searchDataGridView.Rows.Add();
                for (int x = 0; x < row.Count; x++)
                {
                    searchDataGridView[x, row_index].Value = row[x];
                }
                row_index = row_index + 1;
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            searchResultsTable = searchableTable.Search(searchValueTextBox.Text,
                                                        searchTypeComboBox.Text);
            RefreshSearchDataGridView();
        }
    }
}
