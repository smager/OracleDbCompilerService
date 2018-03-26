namespace OracleDbCompilerService
{
    public delegate void SQLProcessDone(string responseText, string developer);
    public delegate void FileProcessDone(string responseText, string sourceFileName);
    public delegate void FolderComplete(string directory, string[] allFiles, string dbFileGroup);
    delegate void Function(); 
}
