using System;
namespace OracleDbCompilerService
{
    public class MasterCompiler
    {
        public string newLine = "\r\n";
        public string sourceFileName { get; set; }
        public string logonName { get; set; }
        public string responseText { get; set; }

        public virtual void Execute()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;

            p.StartInfo.FileName = "compile.bat";
            p.StartInfo.Arguments = string.Format("{0} {1} ", this.logonName,this.sourceFileName);

            p.EnableRaisingEvents = true;
            p.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(Compile_OutputDataReceived);
            p.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(Compile_ErrorDataReceived);
            p.Exited += new EventHandler(Compile_Exited);
            p.Start();
            p.StandardInput.WriteLine("exit");
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();
        }
        public virtual void Compile_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                this.responseText += e.Data + newLine;
            }
        }

        public virtual void Compile_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data.Contains("cannot find")) return;
                this.responseText += e.Data + newLine;
            }
        }

        public virtual void Compile_Exited(object sender, EventArgs e)
        {
            this.responseText += "-----done----" + newLine;
            //this.onSQLProcessDone(this.responseText, this.developer);
        }

    }
}
