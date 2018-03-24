using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace OracleDbCompilerService
{
    public class DataHelper
    {
        public static DataTable getData(string SQL)
        {
            SqlConnection conn = new SqlConnection(AppConfig.ConnectionString);
            conn.Open();
            SqlDataAdapter adap = new SqlDataAdapter(SQL, conn);
            DataTable dt = new DataTable();
            adap.Fill(dt);
            conn.Close();
            return dt;

        }

        public static void Execute(string sql)
        {
            SqlConnection conn = new SqlConnection(AppConfig.ConnectionString);
            SqlCommand command = new SqlCommand(sql, conn);                
            conn.Open();
            command.ExecuteNonQuery();
        }

        public static void Execute(string procedure, SqlParameterCollection parameters)
        {
            SqlConnection conn = new SqlConnection(AppConfig.ConnectionString);
            SqlCommand command = new SqlCommand(procedure, conn);
            command.CommandType = CommandType.StoredProcedure;
            foreach (var p in parameters) {
                command.Parameters.Add(p);
            }
            conn.Open();
            command.ExecuteNonQuery();
        }

    }
    public static class AppConfig
    {
        public static string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["Constr"].ToString(); }
        }
        public static string Developer
        {
            get { return ConfigurationManager.AppSettings["Developer"].ToString(); }
        }
        public static int TimerInterval
        {
            get { return System.Convert.ToInt32(ConfigurationManager.AppSettings["TimerInterval"]); }
        }

        public static string SharedFolder
        {
            get { return ConfigurationManager.AppSettings["SharedFolder"].ToString(); }
        }

    }

}
