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
    public partial class TableFieldForm : Form
    {
        DBTable table;

        public TableFieldForm(String tableName, DBTable new_table)
        {
            InitializeComponent();
            table = new_table;
            this.Text = tableName;
            addFieldButton.Enabled = false;
            deleteFieldButton.Enabled = false;

            // Fill ComboBox options with supported types
            List<Element> supportedTypes = new List<Element>()
            {
                new EInteger(),
                new EReal(),
                new EChar(),
                new EString(),
                //new ETextFile(),
                //new EIntegerInterval(),
                new EComplexInteger(),
                new EComplexReal()
            };
            foreach (Element el in supportedTypes)
            {
                fieldTypeComboBox.Items.Add(el.GetTypeName());
            }
            // Select first type as a default option
            fieldTypeComboBox.Text = fieldTypeComboBox.Items[0].ToString();

            createTableButton.Enabled = false;
            createTableButton.DialogResult = DialogResult.OK;
        }

        private void addFieldButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fieldNameTextBox.Text))
            {
                MessageBox.Show("Field name cannot be empty");
            }
            else
            {
                table.AddField(fieldNameTextBox.Text, fieldTypeComboBox.Text);
                RefreshTableFieldsListBox();
                fieldNameTextBox.Clear();
                addFieldButton.Enabled = false;
            }
        }

        private void deleteFieldButton_Click(object sender, EventArgs e)
        {
            table.DeleteField(tableFieldsListBox.SelectedIndex);
            RefreshTableFieldsListBox();
            deleteFieldButton.Enabled = false;
        }

        /// <summary>
        /// Fetches field descriptions from the table
        /// and shows them in tableFieldsListBox
        /// Updates "Create Table" button
        /// </summary>
        private void RefreshTableFieldsListBox()
        {
            tableFieldsListBox.Items.Clear();
            List<string> fieldList = table.GetFieldList();
            foreach (string fieldDesc in fieldList)
            {
                tableFieldsListBox.Items.Add(fieldDesc);
            }
            // Only enable "Create Table" button if there is at least one field
            createTableButton.Enabled = (fieldList.Count > 0);
        }

        private void tableFieldsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Activate "Delete field" button only if a field is selected
            deleteFieldButton.Enabled = (tableFieldsListBox.SelectedIndex != -1);
        }

        private void fieldNameTextBox_TextChanged(object sender, EventArgs e)
        {
            // Activate "Add field" button only if a field name is not empty
            addFieldButton.Enabled = !string.IsNullOrEmpty(fieldNameTextBox.Text);
        }
    }
}
