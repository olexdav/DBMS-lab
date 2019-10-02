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
    public class DBTableTests
    {
        [TestMethod()]
        public void SearchTest()
        {
            DBTable searchTable = new DBTable();
            DBTable resultTable = new DBTable();
            searchTable.AddField("field", "String");
            resultTable.AddField("field", "String");
            DBRow row = new DBRow();
            EString el = new EString("RISC");
            row.AddElement(el);
            searchTable.AddRow(row);
            resultTable.AddRow(row);
            row = new DBRow();
            el = new EString("CISC");
            row.AddElement(el);
            searchTable.AddRow(row);
            Assert.AreEqual(resultTable, searchTable.Search("R", "String"));
        }

        [TestMethod()]
        public void CompareTablesTest()
        {
            DBTable lTable = new DBTable();
            DBTable rTable = new DBTable();
            lTable.AddField("field", "String");
            rTable.AddField("field", "String");
            EString el = new EString("Sample");
            DBRow row = new DBRow();
            row.AddElement(el);
            lTable.AddRow(row);
            el = new EString("Sample");
            row = new DBRow();
            row.AddElement(el);
            rTable.AddRow(row);
            Assert.AreEqual(lTable, rTable);
        }
    }
}