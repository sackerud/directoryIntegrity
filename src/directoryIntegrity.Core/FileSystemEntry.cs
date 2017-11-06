using System.Collections.Generic;

namespace directoryIntegrity.Core
{
    public class FileSystemEntry : IFileSystemEntry
    {
        public string Path { get; set; }

        public IList<IFileSystemEntry> Children { get; } = new List<IFileSystemEntry>();

        public FileSystemEntry(string rootPath)
        {
            Path = rootPath;
        }

        public void AddChild(IFileSystemEntry child)
        {
            Children.Add(child);
        }
    }
}