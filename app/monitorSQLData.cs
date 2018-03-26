using System;
using System.IO;
using System.Threading;
using System.Data.SqlClient;
using System.Data;

namespace OracleDbCompilerService
{
    public class monitorSQLData 
    {
        string subFolder = "result";
        string logFile { get; set; }
        private System.Timers.Timer timer  { get;set; }
        public monitorSQLData() {
            if (!Directory.Exists(this.subFolder)) Directory.CreateDirectory(this.subFolder);
            this.logFile = this.subFolder + @"\log.txt";
            if (File.Exists(this.logFile)) File.Delete(this.logFile);

            timer = new System.Timers.Timer(AppConfig.TimerInterval);
            timer.Elapsed += OnTimerElapsed;
            timer.Enabled = true;
        }
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("monitorSQLData: " + DateTime.Now.ToString("h:mm:ss tt") );
            this.timer.Stop();
            checkDataFromServer();
        }
        private void checkDataFromServer()
        {
            try
            {
                DataTable dt = DataHelper.getData("select * from oracle_compile_scripts where developer='" + AppConfig.Developer + "'");
                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    if (row["source"].ToString() != "")
                        CompileData(dt, row["file_type"].ToString());
                    else
                        this.timer.Start();
                }
                else
                    this.timer.Start();
            }
            catch (Exception ex)
            {
                File.WriteAllText(this.logFile, ex.Message);
                this.timer.Start();
            }
        }
        private void CompileData(DataTable dataTable,string fileType)
        {
            DataRow dr = dataTable.Rows[0];
            string _developer = dr["developer"].ToString();
            string _sourceFileName = string.Format("{0}\\{1}", this.subFolder, _developer + "-source." + fileType.ToLower());
            File.WriteAllText(_sourceFileName, dr["source"].ToString());
            //start compile
            ThreadStart threadStart = new ThreadStart(new CompileOracleFile(dr["server_user"].ToString(), _sourceFileName,_developer, onSQLProcessDone).Execute);
            Thread t = new Thread(threadStart);
            t.Start();

        }

     
        private void onSQLProcessDone(string responseText, string developer)
        {

            SqlConnection conn = new SqlConnection(AppConfig.ConnectionString);
            SqlCommand command = new SqlCommand("dbo.oracle_compile_scripts_response_upd", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@developer", SqlDbType.VarChar).Value = developer;
            command.Parameters.Add("@response", SqlDbType.VarChar).Value = responseText;
            conn.Open();
            command.ExecuteNonQuery();
            this.timer.Start();
        }

    }
}
