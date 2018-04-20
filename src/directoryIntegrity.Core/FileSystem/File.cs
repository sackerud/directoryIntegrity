namespace directoryIntegrity.Core.FileSystem
{
    public class File : GenericFileSystemEntry {
        public File(string rootPath) : base(rootPath) {}
        public override string Name => System.IO.Path.GetFileName(Path);
        public override bool IsDirectory
        {
            get => false;
            set { /* Setter needed to enable deserialization, must find a way around this... */ }
        }
    }
}