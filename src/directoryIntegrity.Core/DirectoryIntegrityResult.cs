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

        public int AddedDirectoriesCount =>
            ComparisonRows.Count(Directories(FileSystemEntryComparisonResult.Added));

        public int AddedFilesCount =>
            ComparisonRows.Count(Files(FileSystemEntryComparisonResult.Added));

        public int IntactDirectoriesCount =>
            ComparisonRows.Count(Directories(FileSystemEntryComparisonResult.Intact));

        public int IntactFilesCount =>
            ComparisonRows.Count(Files(FileSystemEntryComparisonResult.Intact));

        public int RemovedDirectoriesCount =>
            ComparisonRows.Count(Directories(FileSystemEntryComparisonResult.Removed));

        public int RemovedFilesCount =>
            ComparisonRows.Count(Files(FileSystemEntryComparisonResult.Removed));

        public int MovedDirectoriesCount =>
            ComparisonRows.Count(Directories(FileSystemEntryComparisonResult.Moved));

        public int MovedFilesCount =>
            ComparisonRows.Count(Files(FileSystemEntryComparisonResult.Moved));

        public int TotalDirectoryCount => ComparisonRows.Count(r => r.ReferenceFileSystemEntry.IsDirectory);

        public int TotalFileCount => ComparisonRows.Count(r => !r.ReferenceFileSystemEntry.IsDirectory);

        private static Func<FileSystemEntryComparison, bool> Directories(FileSystemEntryComparisonResult comparisonResult)
        {
            return r => r.Result == comparisonResult
                        && r.ReferenceFileSystemEntry.IsDirectory;
        }

        private static Func<FileSystemEntryComparison, bool> Files(FileSystemEntryComparisonResult comparisonResult)
        {
            return r => r.Result == comparisonResult
                        && !r.ReferenceFileSystemEntry.IsDirectory;
        }
    }
}