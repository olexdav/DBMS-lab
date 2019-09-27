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
        public SearchForm()
        {
            InitializeComponent();
            searchButton.Enabled = false;
        }

        private void FieldNameTextBox_TextChanged(object sender, EventArgs e)
        {
            searchButton.Enabled = (fieldNameTextBox.Text.Length > 0);
        }
    }
}
