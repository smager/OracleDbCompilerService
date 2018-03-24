using System;
using System.IO;
using System.Threading;
using System.Data.SqlClient;
using System.Data;

namespace OracleDbCompilerService
{
    public delegate void SQLProcessDone(string responseText, string developer);
    public delegate void FileProcessDone(string responseText, string sourceFileName);
    delegate void Function(); 
}
