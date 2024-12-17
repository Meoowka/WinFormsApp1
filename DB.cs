
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace WinFormsApp1
{
    class DB
    {
        static string dbConnection = @"Server=meoowka\SQLEXPRESS;Database=MicroSystemTechDB;Integrated Security=True;TrustServerCertificate=True;";
        static public SqlDataAdapter sqlDataAdapter;
        static SqlConnection sqlConnection;
        static public SqlCommand sqlCommand;

        public static bool ConnectionBd()
        {
            try
            {
                sqlConnection = new SqlConnection(dbConnection);
                sqlConnection.Open();
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                return true;
            }
            catch
            {
                MessageBox.Show("Error connection!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static void CloseConnection()
        {
            if (sqlConnection != null && sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public static SqlConnection GetSqlConnection() => sqlConnection;


        ////SqlConnection sqlConnection = new SqlConnection(@"Data Source=meoowka\\sqlexpress; Initial Catalog=MicroSystemTechDB; Integrated Security=True");
        //static string dbConnection = @"Server=YOUR_SERVER_NAME;Database=MicroSystemTechDB;Integrated Security=True;";
        //static public MySqlDataAdapter mySqlDataAdapter;
        //static MySqlConnection MySqlDataConnection;
        //static public MySqlCommand MySqlCommand;

        //public static bool ConnectionBd()
        //{
        //    try
        //    {
        //        MySqlDataConnection = new MySqlConnection(dbConnection);
        //        MySqlDataConnection.Open();
        //        MySqlCommand = new MySqlCommand();
        //        MySqlCommand.Connection = MySqlDataConnection;
        //        mySqlDataAdapter = new MySqlDataAdapter(MySqlCommand);
        //        return true;
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Error connection!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
        //        return false;
        //    }
        //}


        //public void closeCon()
        //{
        //    MySqlDataConnection.Close(); 
        //}
        //public MySqlConnection getSqlConnection() { return MySqlDataConnection; }


    }
}
