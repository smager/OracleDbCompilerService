using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
 

namespace OracleDbCompilerService
{

    public class CompileDirectory
    {
        private FolderComplete onFolderComplete { get; set; }
        private string dbFileGroup { get; set; }
        private List<string> files { get; set; }
        private string[] allFiles { get; set; }
        private string directory { get; set; }

        int counter = 0;
        public CompileDirectory(string directory, string dbFileGroup, string[] allFiles, List<string> files, FolderComplete onFolderComplete) {
            this.dbFileGroup = dbFileGroup;
            this.allFiles = allFiles;
            this.files = files;
            this.directory = directory;
            this.onFolderComplete = onFolderComplete;
            compileFiles();

        }
        
        private void compileFiles() {
            foreach (var f in files)
            {
                ThreadStart threadStart = new ThreadStart(new CompileDirectoryFile(getLogonName(f), f, onFileDone).Execute);
                Thread t = new Thread(threadStart);
                t.Start();
            }
        }
     
        private string getLogonName(string path) {
            char _slash = '\\';
            string[] _arr = path.Split(_slash)[path.Split(_slash).Count() - 2].Split('-');
            return _arr[0] + "/" + _arr[1] + "@" + _arr[2];
        }  
        private void saveResult(string path,string responseText) {

            var resultFolder = Path.GetDirectoryName(path) + @"\result\";
            if (!Directory.Exists(resultFolder)) Directory.CreateDirectory(resultFolder);
            File.WriteAllText(resultFolder + Path.GetFileName(path) + ".txt", responseText);
        }
 
        private void onFileDone(string responseText,string sourceFileName)
        {

            saveResult(sourceFileName, responseText);

            counter++;
            if(counter == this.files.Count)
            {
                Console.WriteLine("header files completed.");
                this.onFolderComplete(this.directory,this.allFiles, this.dbFileGroup);
            }
        }



    }
}
