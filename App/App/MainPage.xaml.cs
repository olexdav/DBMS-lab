using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
//using System.Web.Script.Serialization;
//using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace App
{
    public partial class MainPage : ContentPage
    {
        int refresh_i; // track amount of refreshes

        public MainPage()
        {
            InitializeComponent();

            refresh_i = 0;

            var table = new TableView();
            table.Intent = TableIntent.Data;
            var layout = new StackLayout() { Orientation = StackOrientation.Vertical };
            layout.Children.Add(new Label() { Text = "" });
            table.Root = new TableRoot()
            {
                new TableSection("Press REFRESH to load table")
                {
                    new ViewCell() { View = layout }
                }
            };

            var mainlayout = new StackLayout() { Orientation = StackOrientation.Vertical };
            mainlayout.Children.Add(table);
            var refreshbutton = new Button() { Text = "Refresh", BackgroundColor = Color.DarkSalmon };
            refreshbutton.Clicked += delegate
            {
                RefreshTable();
            };
            mainlayout.Children.Add(refreshbutton);

            Content = mainlayout;
        }

        private void RefreshTable()
        {
            refresh_i += 1;

            string kittenDBLink = "https://drive.google.com/uc?export=download&id=109GsRUBeYYndHjgX5fFEJ-bMm5YWegEW";
            string kittenData = System.Text.Encoding.UTF8.GetString(new WebClient().DownloadData(kittenDBLink));

            Database db = new Database();
            db.LoadFromString(kittenData);

            var tableview = new TableView();
            tableview.Intent = TableIntent.Data;

            tableview.Root = new TableRoot();
            foreach (DBTable table in db.GetTables()) // add each table as a section to the tableView
            {
                string tableName = table.GetName();
                var verlayout = new StackLayout() { Orientation = StackOrientation.Vertical };
                var fieldlayout = new StackLayout() { Orientation = StackOrientation.Horizontal };
                foreach (DBField field in table.GetFields()) // add fields
                {
                    fieldlayout.Children.Add(new Label()
                    {
                        Text = field.GetName(),
                        TextColor = Color.MediumVioletRed,
                        WidthRequest = 200,
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center
                    });
                }
                verlayout.Children.Add(fieldlayout);
                foreach (DBRow row in table.GetRows()) // add each row
                {
                    var horlayout = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal
                    };
                    foreach (Element el in row.GetElements())
                        horlayout.Children.Add(new Label()
                        {
                            Text = el.ToString(),
                            WidthRequest = 200,
                            HorizontalOptions = LayoutOptions.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        });
                    verlayout.Children.Add(horlayout);
                }
                //verlayout.Children.Add(new Label() { Text = "table" });
                tableview.Root.Add(new TableSection(tableName) { new ViewCell() { View = verlayout } });
            }

            var mainlayout = new StackLayout() { Orientation = StackOrientation.Vertical };
            mainlayout.Children.Add(tableview);
            var buttonColor = refresh_i % 2 == 0 ? Color.DarkSalmon : Color.DarkSalmon;
            var refreshbutton = new Button() { Text = "Refresh", BackgroundColor = buttonColor };
            refreshbutton.Clicked += delegate
            {
                RefreshTable();
            };
            mainlayout.Children.Add(refreshbutton);

            Content = mainlayout;
        }

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

            public string GetName()
            {
                return name;
            }

            public string GetSaveFilename()
            {
                return saveFilename;
            }

            public void AddTable(DBTable table)
            {
                tables.Add(table);
            }

            public List<DBTable> GetTables()
            {
                return tables;
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

            public void LoadFromJSON(string path)
            {
                string serializedDB = System.IO.File.ReadAllText(path);
                saveFilename = path;
                LoadFromString(serializedDB);
            }

            public void LoadFromString(string serializedDB)
            {
                //var serializer = new JavaScriptSerializer();
                // Load file DB into a new object
                //Database file_DB = serializer.Deserialize<Database>(serializedDB);
                Database file_DB = JsonConvert.DeserializeObject<Database>(serializedDB);
                name = file_DB.GetName();
                // Copy the new DB's tables
                tables = file_DB.DeserializeTables();
            }

            private void Serialize()
            {
                foreach (DBTable table in tables)
                    table.Serialize();
            }

            private List<DBTable> DeserializeTables()
            {
                foreach (DBTable table in tables)
                    table.Deserialize();
                return tables;
            }

            public Object GraphQLQuery(string query)
            {
                int bracketPos = query.IndexOf('{');
                string tableName = query.Substring(0, bracketPos - 1);
                string fields = query.Substring(bracketPos + 1, query.Length - bracketPos - 2);
                fields = fields.Replace("\t", "");
                if (fields[0] == '\n') // crop edge newline symbols
                    fields = fields.Substring(1);
                if (fields[fields.Length - 1] == '\n')
                    fields = fields.Substring(0, fields.Length - 1);
                foreach (DBTable table in tables) // find table with the right name
                    if (table.GetName().Equals(tableName))
                        return new Dictionary<string, Object>()
                    {
                        {  tableName, table.GraphQLQuery(fields.Split('\n')) }
                    };
                return "Table not found";
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

            /// <summary>
            /// Returns true if the contents of two tables are the same
            /// ! even if the names of two tables differ !
            /// </summary>
            public override bool Equals(object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    DBTable other = (DBTable)obj;
                    var rfields = other.GetFields();
                    var rrows = other.GetRows();
                    // Check for equal table size
                    if (fields.Count != rfields.Count ||
                        rows.Count != rrows.Count)
                        return false;
                    // Check for same fields and rows
                    for (int i = 0; i < fields.Count; i++)
                        if (!fields[i].Equals(rfields[i]))
                            return false;
                    for (int i = 0; i < rows.Count; i++)
                        if (!rows[i].Equals(rrows[i]))
                            return false;
                    // All checks passed, tables are equal
                    return true;
                }
            }

            public void AddField(string fname, string ftype)
            {
                fields.Add(new DBField(fname, ftype));
            }

            public void AddField(DBField field)
            {
                fields.Add(field);
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

            public List<DBField> GetFields()
            {
                return fields;
            }

            public List<DBRow> GetRows()
            {
                return rows;
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

            public void Deserialize()
            {
                foreach (DBRow row in rows)
                    row.Deserialize(fields);
            }

            public DBTable Search(string value, string typeName)
            {
                DBTable resultTable = new DBTable();
                // Add same fields (Warning: no copy constructor!)
                resultTable.fields = fields;
                foreach (DBRow row in rows)
                {
                    bool found = false;
                    for (int i = 0; i < fields.Count; i++)
                        // check if any field of a proper type includes value
                        if (fields[i].GetTypeName().Equals(typeName))
                            if (row.GetElement(i).ToString().IndexOf(value) != -1)
                            {
                                found = true;
                                break;
                            }
                    if (found)
                        resultTable.AddRow(row);
                }
                return resultTable;
            }

            public List<Object> GraphQLQuery(string[] fieldNames)
            {
                //return "TABLE";
                List<int> fieldIndices = new List<int>();
                foreach (string fieldName in fieldNames)
                    for (int i = 0; i < fields.Count; i++)
                        if (fields[i].GetName().Equals(fieldName))
                            fieldIndices.Add(i);
                List<Object> resultRows = new List<Object>();
                foreach (DBRow row in rows)
                {
                    var items = row.GetElements();
                    var values = new Dictionary<string, string>();
                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        values.Add(fieldNames[i], items[fieldIndices[i]].ToString());
                    }
                    resultRows.Add(values);
                }
                return resultRows;
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

            /// <summary>
            /// Returns true if all items in a row have the same content
            /// </summary>
            public override bool Equals(object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    DBRow other = (DBRow)obj;
                    var ritems = other.GetElements();
                    if (items.Count != ritems.Count)
                        return false;
                    // Check that all element have the same values
                    for (int i = 0; i < items.Count; i++)
                        if (!items[i].Equals(ritems[i]))
                            return false;
                    // All checks passed, rows are equal
                    return true;
                }
            }

            public void AddElement(Element el)
            {
                items.Add(el);
            }

            public List<Element> GetElements()
            {
                return items;
            }

            public Element GetElement(int index)
            {
                return items[index];
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
                foreach (Element el in items)
                    serializables.Add(el.ToSerializable());
            }

            public void Deserialize(List<DBField> fields)
            {
                items = new List<Element>();
                for (int i = 0; i < fields.Count; i++)
                {
                    string typeName = fields[i].GetTypeName();
                    var el = Element.GetElementByTypeName(typeName);
                    el.LoadSerializable(serializables[i]);
                    items.Add(el);
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

            public override bool Equals(object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    DBField other = (DBField)obj;
                    return name.Equals(other.GetName()) && typeName.Equals(other.GetTypeName());
                }
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

            public DBField Clone()
            {
                return new DBField(name, typeName);
            }
        }

        public abstract class Element
        {
            public abstract string GetTypeName();
            /// <summary>
            /// Convert this element to a serializable that 
            /// can be saved and loaded from text files
            /// </summary>
            public abstract SerializableElement ToSerializable();
            /// <summary>
            /// Loads the element's values from a serializable element
            /// </summary>
            public abstract void LoadSerializable(SerializableElement serializable);
            public abstract Element Clone();

            public static Element GetElementByTypeName(string typeName)
            {
                switch (typeName)
                {
                    case "Integer":
                        return new EInteger();
                    case "Real":
                        return new EReal();
                    case "Char":
                        return new EChar();
                    case "String":
                        return new EString();
                    case "Text File":
                        return new ETextFile();
                    case "Integer Interval":
                        return new EIntegerInterval();
                    case "Complex Integer":
                        return new EComplexInteger();
                    case "Complex Real":
                        return new EComplexReal();
                    default:
                        return new EString();
                }
            }
            public static List<Element> GetSupportedTypes()
            {
                return new List<Element>()
            {
                new EInteger(),
                new EReal(),
                new EChar(),
                new EString(),
                new ETextFile(),
                new EIntegerInterval(),
                new EComplexInteger(),
                new EComplexReal()
            };
            }
        }

        public class SerializableElement
        {
            public string Data1 { get; set; }
            public string Data2 { get; set; }
        }

        public class EInteger : Element
        {
            int value;

            public EInteger()
            {
                value = 0;
            }

            public EInteger(int val)
            {
                value = val;
            }

            public override bool Equals(object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    EInteger other = (EInteger)obj;
                    return value == other.value;
                }
            }

            public override string ToString()
            {
                return value.ToString();
            }

            public override string GetTypeName()
            {
                return "Integer";
            }

            public override SerializableElement ToSerializable()
            {
                var serializable = new SerializableElement();
                serializable.Data1 = this.ToString();
                return serializable;
            }

            public override void LoadSerializable(SerializableElement serializable)
            {
                value = Int32.Parse(serializable.Data1);
            }

            public override Element Clone()
            {
                EInteger el = new EInteger();
                el.value = value;
                return el;
            }
        }

        public class EReal : Element
        {
            public double value;

            public EReal()
            {
                value = 0.0;
            }

            public override bool Equals(object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    EReal other = (EReal)obj;
                    return value == other.value;
                }
            }

            public override string ToString()
            {
                return value.ToString();
            }

            public override string GetTypeName()
            {
                return "Real";
            }

            public override SerializableElement ToSerializable()
            {
                var serializable = new SerializableElement();
                serializable.Data1 = this.ToString();
                return serializable;
            }

            public override void LoadSerializable(SerializableElement serializable)
            {
                value = Double.Parse(serializable.Data1);
            }

            public override Element Clone()
            {
                EReal el = new EReal();
                el.value = value;
                return el;
            }
        }

        public class EChar : Element
        {
            public char value;

            public EChar()
            {
                value = '#';
            }

            public override bool Equals(object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    EChar other = (EChar)obj;
                    return value == other.value;
                }
            }

            public override string ToString()
            {
                return value.ToString();
            }

            public override string GetTypeName()
            {
                return "Char";
            }

            public override SerializableElement ToSerializable()
            {
                var serializable = new SerializableElement();
                serializable.Data1 = this.ToString();
                return serializable;
            }

            public override void LoadSerializable(SerializableElement serializable)
            {
                value = serializable.Data1.ToCharArray()[0];
            }

            public override Element Clone()
            {
                EChar el = new EChar();
                el.value = value;
                return el;
            }
        }

        public class EString : Element
        {
            public string value;

            public EString()
            {
                value = "Empty";
            }

            public override bool Equals(object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    EString other = (EString)obj;
                    return value == other.value;
                }
            }

            public EString(string val)
            {
                value = val;
            }

            public override string ToString()
            {
                return value.ToString();
            }

            public override string GetTypeName()
            {
                return "String";
            }

            public override SerializableElement ToSerializable()
            {
                var serializable = new SerializableElement();
                serializable.Data1 = this.ToString();
                return serializable;
            }

            public override void LoadSerializable(SerializableElement serializable)
            {
                value = serializable.Data1;
            }

            public override Element Clone()
            {
                EString el = new EString();
                el.value = value;
                return el;
            }
        }

        public class ETextFile : Element
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

            public override SerializableElement ToSerializable()
            {
                var serializable = new SerializableElement();
                serializable.Data1 = this.ToString();
                return serializable;
            }

            public override void LoadSerializable(SerializableElement serializable)
            {
                path = serializable.Data1;
            }

            public override Element Clone()
            {
                ETextFile el = new ETextFile();
                el.path = path;
                return el;
            }
        }

        public class EIntegerInterval : Element
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
                return string.Format("[{0};{1}]", a, b);
            }

            public override string GetTypeName()
            {
                return "Integer Interval";
            }

            public override SerializableElement ToSerializable()
            {
                var serializable = new SerializableElement();
                serializable.Data1 = a.ToString();
                serializable.Data2 = b.ToString();
                return serializable;
            }

            public override void LoadSerializable(SerializableElement serializable)
            {
                a = Int32.Parse(serializable.Data1);
                b = Int32.Parse(serializable.Data2);
            }

            public override Element Clone()
            {
                EIntegerInterval el = new EIntegerInterval();
                el.a = a;
                el.b = b;
                return el;
            }
        }

        public class EComplexInteger : Element
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

            public override SerializableElement ToSerializable()
            {
                var serializable = new SerializableElement();
                serializable.Data1 = real.ToString();
                serializable.Data2 = complex.ToString();
                return serializable;
            }

            public override void LoadSerializable(SerializableElement serializable)
            {
                real = Int32.Parse(serializable.Data1);
                complex = Int32.Parse(serializable.Data2);
            }

            public override Element Clone()
            {
                EComplexInteger el = new EComplexInteger();
                el.real = real;
                el.complex = complex;
                return el;
            }
        }

        public class EComplexReal : Element
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

            public override SerializableElement ToSerializable()
            {
                var serializable = new SerializableElement();
                serializable.Data1 = real.ToString();
                serializable.Data2 = complex.ToString();
                return serializable;
            }

            public override void LoadSerializable(SerializableElement serializable)
            {
                real = Double.Parse(serializable.Data1);
                complex = Double.Parse(serializable.Data2);
            }

            public override Element Clone()
            {
                EComplexReal el = new EComplexReal();
                el.real = real;
                el.complex = complex;
                return el;
            }
        }
    }
}
