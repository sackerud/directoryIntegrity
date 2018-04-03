using System.Collections.Generic;
using System.Linq;
using directoryIntegrity.Core.FileSystem;

namespace directoryIntegrity.Core
{
    public static class CompareExtension
    {
        public static IEnumerable<IFileSystemEntry> Traverse(this IFileSystemEntry root)
        {
            var stack = new Stack<IFileSystemEntry>();
            stack.Push(root);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;
                foreach (var child in current.Children)
                    stack.Push(child);
            }
        }

        public static IEnumerable<FileSystemEntryComparison> CompareTo(this IFileSystemEntry referenceEntry,
                                                                       IFileSystemEntry currentEntry)
        {
            var currentEntries = currentEntry.Traverse().ToList();
            var referenceEntries = referenceEntry.Traverse().ToList();

            foreach (var refEntry in referenceEntries)
            {
                if (currentEntries.Any(c => c.Path == refEntry.Path))
                {
                    yield return new FileSystemEntryComparison
                    {
                        Result = FileSystemEntryComparisonResult.Intact,
                        ReferenceFileSystemEntry = refEntry
                    };
                    continue;
                }

                var newLocations = currentEntries.Where(c => c.Name == refEntry.Name).ToList();

                if (newLocations.Any())
                {
                    yield return new FileSystemEntryComparison
                    {
                        Result = FileSystemEntryComparisonResult.Moved,
                        ReferenceFileSystemEntry = refEntry,
                        CurrentFileSystemEntries = newLocations
                    };
                    continue;
                }

                yield return new FileSystemEntryComparison
                {
                    Result = FileSystemEntryComparisonResult.Removed,
                    ReferenceFileSystemEntry = refEntry
                };
            }
        }
    }
}