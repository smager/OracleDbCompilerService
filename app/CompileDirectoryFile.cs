using System;
namespace OracleDbCompilerService
{
    public class CompileDirectoryFile : MasterCompiler
    {
        public FileProcessDone onFileProcessDone { get; set; }
        public CompileDirectoryFile(string logonName, string sourceFileName, FileProcessDone onFileProcessDone)
        {
            this.sourceFileName = sourceFileName;
            this.logonName = logonName;
            this.onFileProcessDone = onFileProcessDone;
        }
 

        public override void Compile_Exited(object sender, EventArgs e)
        {
            this.responseText += "-----done----" + newLine;
            this.onFileProcessDone(this.responseText,this.sourceFileName);
        }

    }
}
