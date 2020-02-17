using RandomDataLister.Model;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace RandomDataLister.Database
{
    public class Query
    {
        SqlConnection conn = new SqlConnection();

        public void CreateConn()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            conn.ConnectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;
                                    AttachDbFilename={path}\Database\Database1.mdf;
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
