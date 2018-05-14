using System.Collections.Generic;
using System.Linq;

namespace directoryIntegrity.Core.FileSystem
{
    public class FileSystemEntryComparison
    {
        public IFileSystemEntry ReferenceFileSystemEntry { get; set; }

        public IEnumerable<IFileSystemEntry> CurrentFileSystemEntries { get; set; }

        public FileSystemEntryComparisonResult Result { get; set; }

        public override string ToString()
        {
            var fsEntry = ReferenceFileSystemEntry == null
                            ? string.Join("/", CurrentFileSystemEntries.Select(c => c.Path))
                            : ReferenceFileSystemEntry.Path;

            return $"{fsEntry} - {Result.ToString()}";
        }
    }
}