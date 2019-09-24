using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS
{
    public class Database
    {
        string name;
        List<DBTable> tables;

        public Database(string dbName)
        {
            name = dbName;
            tables = new List<DBTable>();
        }

        public void AddTable(DBTable table)
        {
            tables.Add(table);
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

        public List<string> GetFieldList()
        {
            List<string> flist = new List<string>();
            foreach (DBField field in fields)
            {
                flist.Add(field.GetDescription());
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

    //class ETextFile: Element
    //{
    //    // TODO
    //}
    //
    //class EIntegerInterval: Element
    //{
    //    // TODO
    //}

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
