using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using directoryIntegrity.Core.FileSystem;

namespace directoryIntegrity.Core
{
    public class DirectoryIntegrityResult
    {
        /// <summary>
        /// Point in time when a directory was compared to a reference file
        /// </summary>
        public DateTime DirectoryIntegrityDate { get; set; }

        public IEnumerable<FileSystemEntryComparison> ComparisonRows { get; set; }

        public int IntactDirectoryCount =>
            ComparisonRows.Count(r => r.Result == FileSystemEntryComparisonResult.Intact);

        public int AdddedDirectoryCount =>
            ComparisonRows.Count(r => r.Result == FileSystemEntryComparisonResult.Added);
    }
}