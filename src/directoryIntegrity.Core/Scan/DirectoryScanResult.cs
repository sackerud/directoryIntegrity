using System;
using System.Collections.Generic;
using directoryIntegrity.Core.FileSystem;

namespace directoryIntegrity.Core.Scan
{
    public class DirectoryScanResult
    {
        public DateTime ScanDate { get; set; }

        public IEnumerable<IFileSystemEntry> FileSystemEntries { get; set; }
    }
}