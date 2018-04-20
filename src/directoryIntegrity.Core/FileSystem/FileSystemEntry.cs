using System.Collections.Generic;
using Newtonsoft.Json;

namespace directoryIntegrity.Core.FileSystem
{
    public abstract class FileSystemEntry : IFileSystemEntry
    {
        public string Path { get; set; }

        public IList<IFileSystemEntry> Children { get; } = new List<IFileSystemEntry>();

        public abstract string Name { get; }

        public abstract bool IsDirectory { get; set; }

        protected FileSystemEntry(string rootPath)
        {
            Path = rootPath;
        }

        public void AddChild(FileSystemEntry child)
        {
            Children.Add(child);
        }
    }

    public class GenericFileSystemEntry : FileSystemEntry
    {
        public GenericFileSystemEntry(string rootPath) : base(rootPath)
        {
        }

        public override string Name => IsDirectory ?
            System.IO.Path.GetDirectoryName(Path) :
            System.IO.Path.GetFileName(Path);

        public override bool IsDirectory { get; set; }
    }
}