using System.Collections.Generic;
using Newtonsoft.Json;

namespace directoryIntegrity.Core.FileSystem
{
    public abstract class FileSystemEntry : IFileSystemEntry
    {
        public string Path { get; set; }

        public IList<IFileSystemEntry> Children { get; } = new List<IFileSystemEntry>();

        public abstract string Name { get; }

        [JsonIgnore]
        public abstract bool IsDirectory { get; }

        protected FileSystemEntry(string rootPath)
        {
            Path = rootPath;
        }

        public void AddChild(IFileSystemEntry child)
        {
            Children.Add(child);
        }
    }

    public class File : FileSystemEntry {
        public File(string rootPath) : base(rootPath) {}
        public override string Name => System.IO.Path.GetFileName(Path);
        public override bool IsDirectory => false;
    }

    public class FileSystemEntryComparison
    {
        public IFileSystemEntry ReferenceFileSystemEntry { get; set; }

        public IEnumerable<IFileSystemEntry> CurrentFileSystemEntries { get; set; }

        public FileSystemEntryComparisonResult Result { get; set; }

        public override string ToString()
        {
            return $"{ReferenceFileSystemEntry.Path} - {Result.ToString()}";
        }
    }
}