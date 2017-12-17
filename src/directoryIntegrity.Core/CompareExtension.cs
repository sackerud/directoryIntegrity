using System;
using System.Collections.Generic;
using System.Linq;

namespace directoryIntegrity.Core
{
    public static class CompareExtension
    {
        public static IEnumerable<FileSystemEntryComparison> CompareTo(this IEnumerable<FileSystemEntry> referenceEntries,
                                                                            IEnumerable<FileSystemEntry> currentEntries)
        {
            foreach (var referenceEntry in referenceEntries)
            {
                if (currentEntries.Any(c => c.Path == referenceEntry.Path))
                {
                    yield return new FileSystemEntryComparison
                    {
                        Result = FileSystemEntryComparisonResult.Intact,
                        ReferenceFileSystemEntry = referenceEntry,
                    };
                }
            }
        }
    }
}