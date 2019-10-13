using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBMS;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        Database db;

        public ValuesController()
        {
            db = new Database();
            db.LoadFromJSON("C:/Users/olexd/Documents/TarasShevchenkoKNU/Semester 7/Information Technology/DBMS/DBMS/databases/Kitten-DB.txt");
        }

        // GET api/values
        public IEnumerable<Object> Get()
        {
            var totalList = new List<Object>();
            var tableDescList =  db.GetTableDescList();
            var tables = new List<Dictionary<string, string>>();
            int table_index = 0;
            foreach (string tableDesc in tableDescList)
            {
                var table = new Dictionary<string, string>();
                table.Add("Desc", tableDesc);
                table.Add("GET", "http://localhost:56273/api/values/"+table_index);
                table.Add("DELETE", "http://localhost:56273/api/values/" + table_index);
                tables.Add(table);
                table_index += 1;
            }
            totalList.Add(tables);
            totalList.Add(new Dictionary<string, string>()
            {
                { "POST", "http://localhost:56273/api/values" }
            });
            return totalList;
        }

        // GET api/values/5
        public IEnumerable<IEnumerable<string>> Get(int id)
        {
            return db.GetTable(id).GetTextRepresentation();
        }

        // POST api/values
        public IEnumerable<string> Post([FromBody]string value)
        {
            DBTable table = new DBTable(value);
            db.AddTable(table);
            return db.GetTableDescList();
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public IEnumerable<string> Delete(int id)
        {
            db.DeleteTable(id);
            return db.GetTableDescList();
        }
    }
}
