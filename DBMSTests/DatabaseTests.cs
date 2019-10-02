using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.Tests
{
    [TestClass()]
    public class DatabaseTests
    {
        [TestMethod()]
        public void JoinTablesTest()
        {
            // Create tables
            DBTable leftTable = new DBTable();
            leftTable.AddField("Name", "String");
            leftTable.AddField("ValX", "Integer");
            DBTable rightTable = new DBTable();
            rightTable.AddField("Name", "String");
            rightTable.AddField("ValY", "Integer");
            DBTable joinedTable = new DBTable();
            joinedTable.AddField("Name", "String");
            joinedTable.AddField("ValX", "Integer");
            joinedTable.AddField("ValY", "Integer");
            // Fill tables
            DBRow row = new DBRow();
            row.AddElement(new EString("A"));
            row.AddElement(new EInteger(1));
            leftTable.AddRow(row);
            row = new DBRow();
            row.AddElement(new EString("B"));
            row.AddElement(new EInteger(2));
            leftTable.AddRow(row);
            row = new DBRow();
            row.AddElement(new EString("B"));
            row.AddElement(new EInteger(3));
            rightTable.AddRow(row);
            row = new DBRow();
            row.AddElement(new EString("C"));
            row.AddElement(new EInteger(4));
            rightTable.AddRow(row);
            row = new DBRow();
            row.AddElement(new EString("B"));
            row.AddElement(new EInteger(2));
            row.AddElement(new EInteger(3));
            joinedTable.AddRow(row);
            Database db = new Database();
            db.JoinTables(leftTable, 0, rightTable, 0);
            Assert.AreEqual(joinedTable, db.GetTable(0));
        }
    }
}