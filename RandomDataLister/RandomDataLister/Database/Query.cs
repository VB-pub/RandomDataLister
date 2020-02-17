using RandomDataLister.Model;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataLister.Database
{
    public class Query
    {
        SqlConnection conn = new SqlConnection();

        public void CreateConn()
        {
            conn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                                    AttachDbFilename=C:\Users\Vaidas\Desktop\DEV\RandomDataLister\RandomDataLister\RandomDataLister\Database\Database1.mdf;
                                    Integrated Security=True;
                                    pooling=true";
            conn.Open();
        }
        public void CloseConn()
        {
            conn.Close();
        }

        public void Write(RandomData data)
        {
            string query = $@"INSERT RandomData (ThreadId, Data, Time) VALUES ('{data.ThreadId}', '{data.Data}', '{data.Time}')";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
        }

    }
}
