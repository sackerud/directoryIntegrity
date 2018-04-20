using System.Collections.Generic;

namespace directoryIntegrity.Core.FileSystem
{
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