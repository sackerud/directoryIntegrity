using System;
using System.Collections.Generic;
using System.Linq;
using directoryIntegrity.Core.FileSystem;

namespace directoryIntegrity.Core
{
    public class DirectoryIntegrityResult
    {
        public DirectoryIntegrityResult(IEnumerable<FileSystemEntryComparison> comparisonRows)
        {
            ComparisonRows = comparisonRows;
            DirectoryIntegrityDate = DateTime.Now;
        }

        /// <summary>
        /// Point in time when a directory was compared to a reference file
        /// </summary>
        public DateTime DirectoryIntegrityDate { get; set; }

        public IEnumerable<FileSystemEntryComparison> ComparisonRows { get; set; }

        public int IntactDirectoryCount =>
            ComparisonRows.Count(Directories(FileSystemEntryComparisonResult.Added));

        public int AdddedDirectoryCount =>
            ComparisonRows.Count(Directories(FileSystemEntryComparisonResult.Added));

        public int RemovedDirectoryCount =>
            ComparisonRows.Count(Directories(FileSystemEntryComparisonResult.Removed));

        public int MovedDirectoryCount =>
            ComparisonRows.Count(Directories(FileSystemEntryComparisonResult.Moved));

        public int TotalDirectoryCount => ComparisonRows.Count(r => r.ReferenceFileSystemEntry.IsDirectory);

        private static Func<FileSystemEntryComparison, bool> Directories(FileSystemEntryComparisonResult comparisonResult)
        {
            return r => r.Result == comparisonResult
                        && !r.ReferenceFileSystemEntry.IsDirectory;
        }
    }
}