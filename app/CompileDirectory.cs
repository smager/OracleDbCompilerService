using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
 

namespace OracleDbCompilerService
{
    public class CompileDirectory
    {
        public List<string> headerFiles = new List<string>();
        public List<string> bodyFiles = new List<string>();
        public List<string> dbFiles = new List<string>();
        int counterHF = 0
        , counterBF = 0
        , counterF = 0;
        public CompileDirectory(List<string> headerFiles, List<string> bodyFiles, List<string> dbFiles) {
            this.headerFiles = headerFiles;
            this.bodyFiles = bodyFiles;
            this.dbFiles = dbFiles;
            compileHeaderfiles();
        }
        
        private void compileHeaderfiles() {
            foreach (var f in headerFiles)
            {
                ThreadStart threadStart = new ThreadStart(new CompileDirectoryFile(getLogonName(f), f, onHeaderFileDone).Execute);
                Thread t = new Thread(threadStart);
                t.Start();
            }
        }
        private void compileBodyfiles()
        {
            foreach (var f in bodyFiles)
            {
                ThreadStart threadStart = new ThreadStart(new CompileDirectoryFile(getLogonName(f), f, onBodyFileDone).Execute);
                Thread t = new Thread(threadStart);
                t.Start();
            }
        }
        private void compileDbfiles()
        {
            foreach (var f in dbFiles)
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
 
        private void onHeaderFileDone(string responseText,string sourceFileName)
        {

            saveResult(sourceFileName, responseText);

            counterHF++;
            if(counterHF == headerFiles.Count)
            {
                Console.WriteLine("header files completed.");
            }
        }

        private void onBodyFileDone(string responseText, string sourceFileName)
        {

            counterBF++;
            if (counterBF == bodyFiles.Count)
            {
                Console.WriteLine("body files completed.");
            }
 
        }

        private void onFileDone(string responseText, string sourceFileName)
        {

            
            counterF++;
            if (counterF == dbFiles.Count)
            {
                Console.WriteLine("db Files completed.");
            }
           
        }

    }
}
