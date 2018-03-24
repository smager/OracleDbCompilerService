using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
 

namespace OracleDbCompilerService
{
    public class monitorDirectory 
    {
        string subFolder = "result";
        string[] dbTypeFiles = new string[] { "*.sql", "*.plb", "*.psp" };
        string logFile { get; set; }
        private System.Timers.Timer timer = new System.Timers.Timer();
        public monitorDirectory() {
            if (!Directory.Exists(this.subFolder)) Directory.CreateDirectory(this.subFolder);
            this.logFile = this.subFolder + @"\log.txt";
            if (File.Exists(this.logFile)) File.Delete(this.logFile);

            timer.Interval = AppConfig.TimerInterval;
            timer.Elapsed += OnTimerElapsed;
            timer.AutoReset = false;
            timer.Start();
        }
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.timer.Stop();
            checkFiles();
        }
        private void checkFiles()
        {
            //try
            //{

                var directories = Directory.GetDirectories(AppConfig.SharedFolder);
                foreach (var dir in directories)
                {
                    string[] allSqlfiles = GetFilesByExtensions(dir, dbTypeFiles);
                    if(allSqlfiles.Length > 0)
                        new CompileDirectory(
                            getLibraryFiles(allSqlfiles, "lib_hdr")
                           , getLibraryFiles(allSqlfiles, "lib_body")
                           , getDbFiles(allSqlfiles)
                        );
                }
            //}
            //catch (Exception ex)
            //{
            //    File.WriteAllText(this.logFile, ex.Message);
            //    this.timer.Start();
            //}
        }
        private string[] GetFilesByExtensions(string path, params string[] extensions)
        {
            IEnumerable<string> _files = Enumerable.Empty<string>();

            foreach (string ext in extensions)
            {
                _files = _files.Concat(Directory.GetFiles(path, ext));
            }
            return _files.ToArray<string>();
        }
        static List<string> getLibraryFiles(string[] files, string searchPattern)
        {
            var result = new List<string>();

            foreach (var f in files)
            {
                if (f.Contains(searchPattern)) result.Add(f);
            }

            return result;
        }
        static List<string> getDbFiles(string[] files)
        {
            var result = new List<string>();

            foreach (var f in files)
            {
                if (!f.Contains("lib_hdr") && !f.Contains("lib_body")) result.Add(f);
            }

            return result;
        }

    
  
    }
}
