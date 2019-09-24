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
        public TableFieldForm(String tableName)
        {
            InitializeComponent();
            this.Text = tableName;

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
        }

        private void addFieldButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine(fieldTypeComboBox.Text);
        }
    }
}
