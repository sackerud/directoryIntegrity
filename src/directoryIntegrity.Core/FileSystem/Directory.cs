namespace directoryIntegrity.Core.FileSystem
{
    public class Directory : GenericFileSystemEntry
    {
        public Directory(string rootPath) : base(rootPath) {}
        public override string Name => System.IO.Path.GetFileName(Path);
        public override bool IsDirectory
        {
            get => true;
            set { /* Setter needed to enable deserialization, must find a way around this... */ }
        }
    }
}