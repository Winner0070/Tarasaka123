using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarasaka.Components
{
    internal class Database
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-1LANN3F\SQLEXPRESS;Initial Catalog=Tarasaka;Integrated Security=True;");

        public void openConnection()
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
        }

        public void closeConnection()
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }


        public SqlConnection getConnection()
        {
            return con;
        }
    }
}
