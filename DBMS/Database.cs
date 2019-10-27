using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.IO;
using System.Data;
using Npgsql;
using MongoDB.Bson;
using MongoDB.Driver;

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
            saveFilename = path;
            Serialize();
            var serializer = new JavaScriptSerializer();
            string serializedResult = serializer.Serialize(this);
            System.IO.File.WriteAllText(path, serializedResult);
        }

        public void LoadFromJSON(string path)
        {
            string serializedDB = System.IO.File.ReadAllText(path);
            var serializer = new JavaScriptSerializer();
            // Load file DB into a new object
            Database file_DB = serializer.Deserialize<Database>(serializedDB);
            name = file_DB.GetName();
            saveFilename = path;
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

        /// <summary>
        /// Inner join for two tables by one field
        /// </summary>
        public void JoinTables(string leftTableName, string leftTableField,
                               string rightTableName, string rightTableField)
        {
            // Find tables by names
            DBTable leftTable = null;
            DBTable rightTable = null;
            foreach (DBTable table in tables)
            {
                if (table.GetName().Equals(leftTableName))
                    leftTable = table;
                if (table.GetName().Equals(rightTableName))
                    rightTable = table;
            }
            if (leftTable == null)
            {
                MessageBox.Show("Could not find table called " + leftTableName);
                return;
            }
            if (rightTable == null)
            {
                MessageBox.Show("Could not find table called " + rightTableName);
                return;
            }
            // Find fields by names
            int leftFieldIndex = -1;
            int rightFieldIndex = -1;
            List<DBField> lfields = leftTable.GetFields();
            for (int i = 0; i < lfields.Count; i++)
                if (lfields[i].GetName().Equals(leftTableField))
                {
                    leftFieldIndex = i;
                    break;
                }
            if (leftFieldIndex == -1)
            {
                MessageBox.Show("Could not find field called " + leftTableField);
                return;
            }
            List<DBField> rfields = rightTable.GetFields();
            for (int i = 0; i < rfields.Count; i++)
                if (rfields[i].GetName().Equals(rightTableField))
                {
                    rightFieldIndex = i;
                    break;
                }
            if (rightFieldIndex == -1)
            {
                MessageBox.Show("Could not find field called " + rightTableField);
                return;
            }
            // Finally join the actual tables
            JoinTables(leftTable, leftFieldIndex,
                       rightTable, rightFieldIndex);
        }

        public void JoinTables(DBTable leftTable, int leftFieldIndex,
                               DBTable rightTable, int rightFieldIndex)
        {
            string joinedTableName = String.Format("{0}-{1}-Join",
                                                   leftTable.GetName(),
                                                   rightTable.GetName());
            DBTable joinedTable = new DBTable(joinedTableName);
            // Build a joined table
            foreach (DBRow lrow in leftTable.GetRows())
            {
                // For each row in leftTable, get value in key field
                Element keyValue = lrow.GetElement(leftFieldIndex);
                // find rows in rightTable that have the same value in key field
                List<DBRow> rightRows = new List<DBRow>();
                foreach (DBRow rrow in rightTable.GetRows())
                {
                    Element rvalue = rrow.GetElement(rightFieldIndex);
                    // Attention! Compare types by their string representation
                    if (keyValue.ToString().Equals(rvalue.ToString()))
                        rightRows.Add(rrow);
                }
                // concatenate leftRow with all rightRows
                foreach (DBRow rrow in rightRows)
                {
                    DBRow conc_row = new DBRow();
                    // Add all fields from left table
                    foreach (Element el in lrow.GetElements())
                        conc_row.AddElement(el.Clone());
                    // Add all fields from right table, except key field
                    for (int i = 0; i < rrow.GetElements().Count; i++)
                        if (i != rightFieldIndex)
                            conc_row.AddElement(rrow.GetElement(i).Clone());
                    joinedTable.AddRow(conc_row);
                }
            }
            // Concatenate fields
            foreach (DBField field in leftTable.GetFields())
                joinedTable.AddField(field.Clone()); // left table
            List<DBField> rfields = rightTable.GetFields();
            for (int i = 0; i < rfields.Count; i++)
                if (i != rightFieldIndex)
                    joinedTable.AddField(rfields[i].Clone());
            // Add new table to database
            tables.Add(joinedTable);
        }

        /// <summary>
        /// Load and populate DB from PostgreSQL
        /// </summary>
        public void LoadFromPostgres()
        {
            try
            {
                string connstring = "Server=127.0.0.1;Port=5432;" +
                    "User Id=kitten_user;Password=KittenPass;Database=KittenDB;";
                // Making connection with Npgsql provider
                NpgsqlConnection conn = new NpgsqlConnection(connstring);
                conn.Open();
                string query = "SELECT table_schema || '.' || table_name    " +
                "FROM information_schema.tables                             " +
                "WHERE table_type = 'BASE TABLE'                            " +
                "AND table_schema NOT IN('pg_catalog', 'information_schema')";
                var command = new NpgsqlCommand(query, conn);
                var reader = command.ExecuteReader();
                var tableNames = new List<string>();
                while (reader.Read())
                {
                    string fullName = reader[0].ToString();
                    string tableName = fullName.Substring(fullName.IndexOf('.') + 1);
                    Console.WriteLine(tableName);
                    tableNames.Add(tableName);
                }
                reader.Close();
                foreach (string tableName in tableNames)
                    tables.Add(DBTable.LoadFromPostgres(tableName, conn));
            }
            catch (Exception Exc)
            {
                Console.WriteLine(Exc.Message);
            }
        }

        /// <summary>
        /// Push the entire database to PostgreSQL
        /// </summary>
        public void SaveToPostgres()
        {
            try
            {
                string connstring = "Server=127.0.0.1;Port=5432;" +
                    "User Id=kitten_user;Password=KittenPass;Database=KittenDB;";
                // Making connection with Npgsql provider
                NpgsqlConnection conn = new NpgsqlConnection(connstring);
                conn.Open();
                // Drop all existing tables
                string query = "DROP SCHEMA public CASCADE; CREATE SCHEMA public;";
                var command = new NpgsqlCommand(query, conn);
                command.ExecuteNonQuery();
                // Save all tables
                foreach (DBTable table in tables)
                    table.SaveToPostgres(conn);
                // we don't need connection anymore
                conn.Close();
            }
            catch (Exception Exc)
            {
                Console.WriteLine(Exc.Message);
            }
        }

        public void LoadFromMongo()
        {
            string connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("test");
            var collection = database.GetCollection<BsonDocument>("kittens");
            // Identity filter to get all documents in a collection
            var filter = Builders<BsonDocument>.Filter.Empty;
            var cursor = collection.Find(filter);
            var docs = cursor.ToList();
            foreach (var doc in docs) // load all tables
                tables.Add(DBTable.LoadFromBSON(doc));
        }

        public void SaveToMongo()
        {
            string connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("test");
            database.DropCollection("kittens");
            database.CreateCollection("kittens");
            var collection = database.GetCollection<BsonDocument>("kittens");
            foreach (DBTable table in tables)
                collection.InsertOne(table.ToBsonDocument());
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
                DBTable other  = (DBTable)obj;
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

        public void SaveToPostgres(NpgsqlConnection conn)
        {
            // Create table
            var fieldList = new List<string>();
            foreach (string fname in GetFieldList())
            {
                fieldList.Add(fname + " character varying(50)");
            }
            string query = "CREATE TABLE " + name +
                " ( " + String.Join(", ", fieldList) + " )";
            //Console.WriteLine(query);
            new NpgsqlCommand(query, conn).ExecuteNonQuery();
            // Insert all rows into table
            var rowList = new List<string>();
            foreach (DBRow row in rows)
            {
                var elementsList = new List<string>();
                foreach (Element el in row.GetElements())
                {
                    elementsList.Add("'" + el.ToString() + "'");
                }
                rowList.Add("(" + String.Join(", ", elementsList) + ")");
            }
            query = "INSERT INTO " + name + " (" + String.Join(", ", GetFieldList()) + 
                ") VALUES " + String.Join(", ", rowList);
            //Console.WriteLine(query);
            new NpgsqlCommand(query, conn).ExecuteNonQuery();
        }

        public static DBTable LoadFromPostgres(string tableName, 
                                               NpgsqlConnection conn)
        {
            DBTable newTable = new DBTable(tableName);
            string query = "SELECT * FROM " + tableName;
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string colName = dt.Columns[i].ColumnName;
                newTable.fields.Add(new DBField(colName, "String"));
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DBRow newRow = new DBRow();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    EString estr = new EString(dt.Rows[i][j].ToString());
                    newRow.AddElement(estr);
                }
                newTable.AddRow(newRow);
            }
            return newTable;
        }

        public static DBTable LoadFromBSON(BsonDocument doc)
        {
            DBTable newTable = new DBTable();
            newTable.name = doc["name"].ToString();
            var bsonFields = doc["fields"].AsBsonArray;
            foreach (var bsonField in bsonFields)
                newTable.AddField(bsonField["name"].ToString(),
                                  bsonField["typeName"].ToString());
            var bsonRows = doc["rows"].AsBsonArray;
            foreach (var bsonRow in bsonRows)
            {
                DBRow newRow = new DBRow();
                var elements = new List<SerializableElement>();
                var bsonElemens = bsonRow["serializables"].AsBsonArray;
                foreach (var bsonElement in bsonElemens)
                {
                    var el = new SerializableElement();
                    el.Data1 = bsonElement["Data1"].ToString();
                    el.Data2 = bsonElement["Data2"].ToString();
                    elements.Add(el);
                }
                newRow.serializables = elements;
                newTable.AddRow(newRow);
            }
            newTable.Deserialize();
            return newTable;
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

        public void InputElement(DBField field)
        {
            string typeName = field.GetTypeName();
            string fieldName = field.GetName();
            Element newElement;
            newElement = Element.GetElementByTypeName(typeName);
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
        /// Show dialog window and input all values into an element
        /// </summary>
        public abstract void Input(string fieldName);
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

    public class EInteger: Element
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

        public override void Input(string fieldName)
        {
            bool validated = false;
            while (!validated)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter integer value for " + fieldName + ":",
                                                                          fieldName, "");
                try
                {
                    value = Int32.Parse(input);
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

    public class EReal: Element
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

        public override void Input(string fieldName)
        {
            bool validated = false;
            while (!validated)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter string value for " + fieldName + ":",
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

        public override void Input(string fieldName)
        {
            bool validated = false;
            while (!validated)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter file name for " + fieldName + ":",
                                                                          fieldName, "");
                if (!File.Exists("../../textfiles/" + input))
                {
                    MessageBox.Show("This text file does not exist!");
                }
                else
                {
                    path = input;
                    validated = true;
                }
            }
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
