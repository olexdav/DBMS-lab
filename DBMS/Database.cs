using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS
{
    class Database
    {
        String name;
        List<DBTable> tables;

        public Database(String dbName)
        {
            name = dbName;
            tables = new List<DBTable>();
        }

        public void AddTable(DBTable table)
        {
            tables.Add(table);
        }
    }

    class DBTable
    {
        String name;
        //List<DBRow> rows;
        public DBTable(String tableName)
        {
            name = tableName;
        }
    }
    
    class DBRow
    {

    }

    class Element
    {

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
    }

    class ETextFile: Element
    {
        // TODO
    }

    class EIntegerInterval: Element
    {
        // TODO
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
    }
}
