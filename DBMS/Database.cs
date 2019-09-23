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
}
