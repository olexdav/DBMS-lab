using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace DBMS
{
    public class Database
    {
        string name;
        string saveFilename;
        List<DBTable> tables;

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
            //string serializedResult = serializer.Serialize(tables[0]);
            string serializedResult = JsonConvert.SerializeObject(tables[0]);
            Console.WriteLine(serializedResult);
        }
    }

    public class DBTable
    {
        string name;
        List<DBField> fields;
        List<DBRow> rows;

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
    }
    
    class DBRow
    {
        List<Element> items;

        public void AddElement(Element el)
        {
            items.Add(el);
        }
    }

    class DBField
    {
        string name;
        string typeName;

        public DBField(string _name, string _typeName)
        {
            name = _name;
            typeName = _typeName;
        }

        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Returns field description in the form of "name: type"
        /// </summary>
        public string GetDescription()
        {
            return name + ": " + typeName;
        }
    }

    abstract class Element
    {
        public abstract string GetTypeName();
    }

    class EInteger: Element
    {
        int value;

        public EInteger()
        {
            value = 0;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override string GetTypeName()
        {
            return "Integer";
        }
    }

    class EReal: Element
    {
        double value;
        
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
    }

    class EChar: Element
    {
        char value;

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
    }

    class EString: Element
    {
        string value;

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
    }

    class ETextFile : Element
    {
        string path;

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
            return "Text";
        }
    }

    class EIntegerInterval : Element
    {
        int a;
        int b;

        public EIntegerInterval()
        {
            a=0;
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
    }

    class EComplexInteger: Element
    {
        int real;
        int complex;

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
    }

    class EComplexReal: Element
    {
        double real;
        double complex;

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
    }
}
