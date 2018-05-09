using System;
using System.Collections.Generic;
using System.Linq;
using directoryIntegrity.Core.FileSystem;
using Newtonsoft.Json;

namespace directoryIntegrity.Core
{
    public static class CompareExtension
    {
        private static StringComparison _stringComparison;

        public static IEnumerable<FileSystemEntryComparison> CompareTo(this IFileSystemEntry referenceEntry,
                                                                       IFileSystemEntry currentEntry)
        {
            var currentEntries = currentEntry.Traverse().ToList();
            var referenceEntries = referenceEntry.Traverse().ToList();

            foreach (var refEntry in referenceEntries)
            {
                if (currentEntries.Any(c =>
                {
                    _stringComparison = StringComparison.OrdinalIgnoreCase;
                    return c.Path.Equals(refEntry.Path, _stringComparison);
                }))
                {
                    yield return new FileSystemEntryComparison
                    {
                        Result = FileSystemEntryComparisonResult.Intact,
                        ReferenceFileSystemEntry = refEntry,
                        CurrentFileSystemEntries = currentEntries.Where(c => c.Path.Equals(refEntry.Path, _stringComparison))
                    };
                    continue;
                }

                var newLocations = currentEntries.Where(c => c.Name.Equals(refEntry.Name, _stringComparison)).ToList();

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

            var addedEntries = currentEntries.Except(referenceEntries, new FileSystemEntryNameComparer()).ToList();

            if (addedEntries.Any())
            {
                foreach (var addedEntry in addedEntries)
                {
                    yield return new FileSystemEntryComparison
                    {
                        Result = FileSystemEntryComparisonResult.Added,
                        ReferenceFileSystemEntry = null,
                        CurrentFileSystemEntries = new List<IFileSystemEntry> { addedEntry }
                    };
                }
            }
        }

        public static IEnumerable<IFileSystemEntry> Deserialize(this string filepath)
        {
            var contents = System.IO.File.ReadAllText(filepath);
            // Must handle deserializing to File/Directory, see: http://skrift.io/articles/archive/bulletproof-interface-deserialization-in-jsonnet/
            var fseList = JsonConvert.DeserializeObject<List<GenericFileSystemEntry>>(contents);
            return fseList;
        }

        public static int CountAllEntries(this IFileSystemEntry listOfEntries)
        {
            return listOfEntries.Traverse().Count();
        }

        private static IEnumerable<IFileSystemEntry> Traverse(this IFileSystemEntry root)
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
    }
}