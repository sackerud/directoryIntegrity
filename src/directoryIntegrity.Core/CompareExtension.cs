using System;
using System.Collections.Generic;

namespace directoryIntegrity.Core
{
    public static class CompareExtension
    {
        public static IEnumerable<FileSystemEntryComparison> CompareTo(this IEnumerable<FileSystemEntry> referenceEntries,
                                                                            IEnumerable<FileSystemEntry> currentEntries)
        {
            throw new NotImplementedException();
        }
    }
}