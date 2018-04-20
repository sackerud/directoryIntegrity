namespace directoryIntegrity.Core.FileSystem
{
    public class Directory : FileSystemEntry
    {
        public Directory(string rootPath) : base(rootPath) {}
        public override string Name => System.IO.Path.GetFileName(Path);
        public override bool IsDirectory => true;
    }
}