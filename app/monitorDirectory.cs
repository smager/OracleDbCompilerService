using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;


namespace OracleDbCompilerService
{
    public class monitorDirectory 
    {
        string subFolder = "result";
        string completedFile = "completed.txt";
        string[] dbTypeFiles = new string[] { "*.sql", "*.plb", "*.psp" };
        string logFile { get; set; }
        private Timer timer { get; set; }


        public monitorDirectory() {
            if (!Directory.Exists(this.subFolder)) Directory.CreateDirectory(this.subFolder);
            this.logFile = this.subFolder + @"\log.txt";
            if (File.Exists(this.logFile)) File.Delete(this.logFile);

            timer = new Timer(AppConfig.TimerInterval);
            timer.Elapsed += OnTimerElapsed;
            timer.Enabled = true;

        }
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

           Console.WriteLine("monitorDirectory interval:" + AppConfig.TimerInterval);
           checkFiles();
        }
 

        private void checkFiles()
        {
            try
            {

                var directories = Directory.GetDirectories(AppConfig.SharedFolder);
                foreach (var dir in directories)
                {
                    var goCommand = dir + @"\go.txt";
                    if(File.Exists(dir + @"\" + completedFile))   File.Delete(dir + @"\" + completedFile);
                if (File.Exists(goCommand))
                    {
                        this.timer.Stop();
                        string[] allSqlfiles = GetFilesByExtensions(dir, dbTypeFiles);
                        if (allSqlfiles.Length > 0) onFolderCompleted(dir, allSqlfiles, "");
                        File.Delete(goCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(this.logFile, ex.Message);
                this.timer.Start();
            }
        }

        private void onFolderCompleted(string directory, string[] allSqlfiles,string dbFileGroup) {
            switch (dbFileGroup) {

                case "":
                    new CompileDirectory(directory, "hdr", allSqlfiles, getLibraryFiles(allSqlfiles, "lib_hdr"), this.onFolderCompleted);
                    break;

                case "hdr":
                        new CompileDirectory(directory, "body", allSqlfiles,getLibraryFiles(allSqlfiles, "lib_body"), this.onFolderCompleted);
                        break;
                case "body":
                        new CompileDirectory(directory, "dbfiles", allSqlfiles, getDbFiles(allSqlfiles), this.onFolderCompleted);
                        break;
                case "dbfiles":
                        File.WriteAllText(directory + @"\" + completedFile, "");
                        this.timer.Start();
                        break;
                default: break;
            }
        
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
