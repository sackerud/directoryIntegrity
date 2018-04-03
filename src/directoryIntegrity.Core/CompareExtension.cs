using System.Collections.Generic;
using System.Linq;

namespace directoryIntegrity.Core
{
    public static class CompareExtension
    {
        public static IEnumerable<FileSystemEntryComparison> CompareTo(this FileSystemEntry referenceEntry, FileSystemEntry currentEntry)
        {
            return referenceEntry.Children.CompareTo(currentEntry.Children);
        }

        public static IEnumerable<FileSystemEntryComparison> CompareTo(this IEnumerable<IFileSystemEntry> referenceEntries,
                                                                       IEnumerable<IFileSystemEntry> currentEntries)
        {
            foreach (var referenceEntry in referenceEntries)
            {
                if (currentEntries.Any(c => c.Path == referenceEntry.Path))
                {
                    yield return new FileSystemEntryComparison
                    {
                        Result = FileSystemEntryComparisonResult.Intact,
                        ReferenceFileSystemEntry = referenceEntry
                    };
                }
                else
                {
                    yield return new FileSystemEntryComparison
                    {
                        Result = FileSystemEntryComparisonResult.Removed,
                        ReferenceFileSystemEntry = referenceEntry
                    };
                }
            }
        }
    }
}