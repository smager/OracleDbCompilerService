using System;
namespace OracleDbCompilerService
{
    public class CompileOracleFile : MasterCompiler
    {
        public SQLProcessDone onSQLProcessDone { get; set; }
        private string developer { get; set; }
        public CompileOracleFile(string logonName, string sourceFileName, string developer, SQLProcessDone onSQLProcessDone)
        {
            this.sourceFileName = sourceFileName;
            this.logonName = logonName;
            this.developer = developer;
            this.onSQLProcessDone = onSQLProcessDone;
        }
 

        public override void Compile_Exited(object sender, EventArgs e)
        {
            this.responseText += "-----done----" + newLine;
            this.onSQLProcessDone(this.responseText, this.developer);
        }

    }
}
