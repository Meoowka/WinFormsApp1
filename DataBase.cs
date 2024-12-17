using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WinFormsApp1
{
    class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=meoowka\\sqlexpress; Initial Catalog=MicroSystemTechDB; Integrated Security=True");

        public void openCon()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed) { sqlConnection.Open(); }
        }
        public void closeCon()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open) { sqlConnection.Close(); }
        }
        public SqlConnection getSqlConnection() { return sqlConnection; }

    }
}
