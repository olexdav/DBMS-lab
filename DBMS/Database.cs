using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace DBMS
{
    public class Database
    {
        public string name;
        public string saveFilename;
        public List<DBTable> tables;

        public Database()
        {
            name = "Nice-Database";
            saveFilename = null;
            tables = new List<DBTable>();
        }

        public Database(string dbName)
        {
            name = dbName;
            saveFilename = null;
            tables = new List<DBTable>();
        }

        public void AddTable(DBTable table)
        {
            tables.Add(table);
        }

        public DBTable GetTable(int index)
        {
            return tables.ElementAt(index);
        }

        public void DeleteTable(int index)
        {
            tables.RemoveAt(index);
        }

        /// <summary>
        /// Get list of table descriptions
        /// </summary>
        public List<string> GetTableDescList()
        {
            List<string> tlist = new List<string>();
            foreach (DBTable table in tables)
            {
                tlist.Add(table.GetDescription());
            }
            return tlist;
        }

        public void SaveToJSON(string path)
        {
            var serializer = new JavaScriptSerializer();
            //string serializedResult = serializer.Serialize(this);
            DBRow test_row = new DBRow();
            EInteger test_element = new EInteger();
            this.Serialize();
            string serializedResult = serializer.Serialize(this);
            //string serializedResult = serializer.Serialize(test_element);
            Console.WriteLine(serializedResult);
            var deserializedResult = serializer.Deserialize<Database>(serializedResult);
            //var deserializedResult = serializer.Deserialize<EInteger>(serializedResult);

            //string serializedResult = JsonConvert.SerializeObject(tables[0]);

            System.IO.File.WriteAllText(path, serializedResult);
        }

        private void Serialize()
        {
            foreach (DBTable table in tables)
                table.Serialize();
        }

        private void Deserialize()
        {
            
        }
    }

    public class DBTable
    {
        public string name;
        public List<DBField> fields;
        public List<DBRow> rows;

        public DBTable()
        {
            name = "Nice-Table";
            fields = new List<DBField>();
            rows = new List<DBRow>();
        }

        public DBTable(string tableName)
        {
            name = tableName;
            fields = new List<DBField>();
            rows = new List<DBRow>();
        }

        public void AddField(string fname, string ftype)
        {
            fields.Add(new DBField(fname, ftype));
        }

        /// <summary>
        /// Prompts user to enter all elements to form a DBRow. Returns this DBRow
        /// </summary>
        public DBRow InputRow()
        {
            DBRow new_row = new DBRow();
            foreach (DBField field in fields)
            {
                new_row.InputElement(field);
            }
            return new_row;
        }

        public void AddRow(DBRow row)
        {
            rows.Add(row);
        }

        public void ReplaceRow(DBRow row, int index)
        {
            rows[index] = row;
        }

        public void DeleteRow(int index)
        {
            rows.RemoveAt(index);
        }

        public void DeleteField(int index)
        {
            fields.RemoveAt(index);
        }

        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Returns table description in the following format
        /// "Name: (field1: type, field2:type)"
        /// </summary>
        public string GetDescription()
        {
            List<string> flist = GetFieldListWithTypes();
            return name + ": (" + string.Join(", ", flist) + ")";
        }

        public List<string> GetFieldListWithTypes()
        {
            List<string> flist = new List<string>();
            foreach (DBField field in fields)
            {
                flist.Add(field.GetDescription());
            }
            return flist;
        }

        public List<string> GetFieldList()
        {
            List<string> flist = new List<string>();
            foreach (DBField field in fields)
            {
                flist.Add(field.GetName());
            }
            return flist;
        }

        /// <summary>
        /// Gets string representation of a table, to be shown in DataGridViews
        /// </summary>
        public List<List<string>> GetTextRepresentation()
        {
            List<List<string>> representation = new List<List<string>>();
            foreach (DBRow row in rows)
            {
                List<string> repr_row = row.GetTextRepresentation();
                representation.Add(repr_row);
            }
            return representation;
        }

        public void Serialize()
        {
            foreach (DBRow row in rows)
                row.Serialize();
        }
    }
    
    public class DBRow
    {
        List<Element> items;
        public List<SerializableElement> serializables;

        public DBRow()
        {
            items = new List<Element>();
            serializables = new List<SerializableElement>();
        }

        public void InputElement(DBField field)
        {
            string typeName = field.GetTypeName();
            string fieldName = field.GetName();
            Element newElement;
            switch (typeName)
            {
                case "Integer":
                    newElement = new EInteger();
                    break;
                case "Real":
                    newElement = new EReal();
                    break;
                case "Char":
                    newElement = new EChar();
                    break;
                case "String":
                    newElement = new EString();
                    break;
                case "Text File":
                    newElement = new ETextFile();
                    break;
                case "Integer Interval":
                    newElement = new EIntegerInterval();
                    break;
                case "Complex Integer":
                    newElement = new EComplexInteger();
                    break;
                case "Complex Real":
                    newElement = new EComplexReal();
                    break;
                default:
                    newElement = new EString();
                    break;
            }
            newElement.Input(fieldName);
            items.Add(newElement);
        }

        public List<string> GetTextRepresentation()
        {
            List<string> representation = new List<string>();
            foreach (Element el in items)
                representation.Add(el.ToString());
            return representation;
        }

        /// <summary>
        /// Fill in the list of serializables so the object can be saved to JSON
        /// </summary>
        public void Serialize()
        {
            serializables = new List<SerializableElement>();
            foreach(Element el in items)
            {
                serializables.Add(el.ToSerializable());
            }
        }
    }

    public class DBField
    {
        public string name;
        public string typeName;

        public DBField()
        {
            name = "Field-Name";
            typeName = "Undefined Type";
        }

        public DBField(string _name, string _typeName)
        {
            name = _name;
            typeName = _typeName;
        }

        public string GetName()
        {
            return name;
        }

        public string GetTypeName()
        {
            return typeName;
        }

        /// <summary>
        /// Returns field description in the form of "name: type"
        /// </summary>
        public string GetDescription()
        {
            return name + ": " + typeName;
        }
    }

    public abstract class Element
    {
        public abstract string GetTypeName();
        /// <summary>
        /// Show dialog window and input all values into an element
        /// </summary>
        public abstract void Input(string fieldName);
        public abstract SerializableElement ToSerializable();
    }

    public class SerializableElement
    {
        public string Data1 { get; set; }
        public string Data2 { get; set; }
    }

    class EInteger: Element
    {
        int val;

        public EInteger()
        {
            val = 0;
        }

        public override string ToString()
        {
            return val.ToString();
        }

        public override string GetTypeName()
        {
            return "Integer";
        }

        public override void Input(string fieldName)
        {
            bool validated = false;
            while (!validated)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter integer value for " + fieldName + ":",
                                                                          fieldName, "");
                try
                {
                    val = Int32.Parse(input);
                    validated = true;
                }
                catch { };
            }
        }

        public override SerializableElement ToSerializable()
        {
            var serializable = new SerializableElement();
            serializable.Data1 = this.ToString();
            return serializable;
        }
    }

    class EReal: Element
    {
        public double value;

        public EReal()
        {
            value = 0.0;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override string GetTypeName()
        {
            return "Real";
        }

        public override void Input(string fieldName)
        {
            bool validated = false;
            while (!validated)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter real value for " + fieldName + ":",
                                                                          fieldName, "");
                try
                {
                    value = Double.Parse(input);
                    validated = true;
                }
                catch { };
            }
        }

        public override SerializableElement ToSerializable()
        {
            var serializable = new SerializableElement();
            serializable.Data1 = this.ToString();
            return serializable;
        }
    }

    class EChar: Element
    {
        public char value;

        public EChar()
        {
            value = '#';
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override string GetTypeName()
        {
            return "Char";
        }

        public override void Input(string fieldName)
        {
            bool validated = false;
            while (!validated)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter character for " + fieldName + ":",
                                                                          fieldName, "");
                try
                {
                    value = Char.Parse(input);
                    validated = true;
                }
                catch { };
            }
        }

        public override SerializableElement ToSerializable()
        {
            var serializable = new SerializableElement();
            serializable.Data1 = this.ToString();
            return serializable;
        }
    }

    class EString: Element
    {
        public string value;

        public EString()
        {
            value = "Empty";
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override string GetTypeName()
        {
            return "String";
        }

        public override void Input(string fieldName)
        {
            bool validated = false;
            while (!validated)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter real value for " + fieldName + ":",
                                                                          fieldName, "");
                try
                {
                    value = input;
                    validated = true;
                }
                catch { };
            }
        }

        public override SerializableElement ToSerializable()
        {
            var serializable = new SerializableElement();
            serializable.Data1 = this.ToString();
            return serializable;
        }
    }

    class ETextFile : Element
    {
        public string path;

        public ETextFile()
        {
            path = "Empty";
        }

        public override string ToString()
        {
            return path.ToString();
        }

        public override string GetTypeName()
        {
            return "Text File";
        }

        public override void Input(string fieldName)
        {
            //TODO
        }

        public override SerializableElement ToSerializable()
        {
            var serializable = new SerializableElement();
            serializable.Data1 = this.ToString();
            return serializable;
        }
    }

    class EIntegerInterval : Element
    {
        public int a;
        public int b;

        public EIntegerInterval()
        {
            a = 0;
            b = 1;
        }

        public override string ToString()
        {
            return string.Format("[{0};{1}]",a,b);
        }

        public override string GetTypeName()
        {
            return "Integer Interval";
        }

        public override void Input(string fieldName)
        {
            bool validated = false;
            while (!validated)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter lower bound for " + fieldName + ":",
                                                                          fieldName, "");
                try
                {
                    a = Int32.Parse(input);
                    validated = true;
                }
                catch { };
            }
            validated = false;
            while (!validated)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter upper bound for " + fieldName + ":",
                                                                          fieldName, "");
                try
                {
                    b = Int32.Parse(input);
                    if (b < a)
                        MessageBox.Show("Upper bound must be higher than lower!");
                    else
                        validated = true;
                }
                catch { };
            }
        }

        public override SerializableElement ToSerializable()
        {
            var serializable = new SerializableElement();
            serializable.Data1 = a.ToString();
            serializable.Data2 = b.ToString();
            return serializable;
        }
    }

    class EComplexInteger: Element
    {
        public int real;
        public int complex;

        public EComplexInteger()
        {
            real = 1;
            complex = 0;
        }

        public override string ToString()
        {
            return string.Format("{0} + {1}i", real, complex);
        }

        public override string GetTypeName()
        {
            return "Complex Integer";
        }

        public override void Input(string fieldName)
        {
            bool validated = false;
            while (!validated)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter real part for " + fieldName + ":",
                                                                          fieldName, "");
                try
                {
                    real = Int32.Parse(input);
                    validated = true;
                }
                catch { };
            }
            validated = false;
            while (!validated)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter complex part for " + fieldName + ":",
                                                                          fieldName, "");
                try
                {
                    complex = Int32.Parse(input);
                    validated = true;
                }
                catch { };
            }
        }

        public override SerializableElement ToSerializable()
        {
            var serializable = new SerializableElement();
            serializable.Data1 = real.ToString();
            serializable.Data2 = complex.ToString();
            return serializable;
        }
    }

    class EComplexReal: Element
    {
        public double real;
        public double complex;

        public EComplexReal()
        {
            real = 1.0;
            complex = 0.0;
        }

        public override string ToString()
        {
            return string.Format("{0} + {1}i", real, complex);
        }

        public override string GetTypeName()
        {
            return "Complex Real";
        }

        public override void Input(string fieldName)
        {
            bool validated = false;
            while (!validated)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter real part for " + fieldName + ":",
                                                                          fieldName, "");
                try
                {
                    real = Double.Parse(input);
                    validated = true;
                }
                catch { };
            }
            validated = false;
            while (!validated)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter complex part for " + fieldName + ":",
                                                                          fieldName, "");
                try
                {
                    complex = Double.Parse(input);
                    validated = true;
                }
                catch { };
            }
        }

        public override SerializableElement ToSerializable()
        {
            var serializable = new SerializableElement();
            serializable.Data1 = real.ToString();
            serializable.Data2 = complex.ToString();
            return serializable;
        }
    }
}
