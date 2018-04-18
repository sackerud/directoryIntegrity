using System;
using System.Collections.Generic;
using System.Linq;
using directoryIntegrity.Core;
using directoryIntegrity.Core.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace directoryIntegrity.UnitTests
{
    [TestClass]
    public class CompareExtensionTest
    {
        private FileSystemEntry _rootDir;

        [TestInitialize]
        public void TestInitialize()
        {
            _rootDir = new Directory("rootDir");
            _rootDir.AddChild(new File("fileAtRootDir"));

            var subDir1 = new Directory("subDir1");
            _rootDir.AddChild(subDir1);
        }

        [TestMethod]
        public void Comparing_identical_filesystem_structures_should_return_only_intact_result()
        {
            // Act
            var actual = _rootDir.CompareTo(_rootDir);

            // Assert
            Assert.IsTrue(actual.All(a => a.Result == FileSystemEntryComparisonResult.Intact),
                string.Join(Environment.NewLine, actual.Select(a => a.Result)));
        }

        [TestMethod]
        public void If_1_file_is_missing_comparison_should_return_removed()
        {
            var root1 = FileSystemEntryBuilder.CreateRoot(@"C:\test");

            root1.AddFile("file1.txt");

            var root2 = FileSystemEntryBuilder.CreateRoot(@"C:\test");

            var actual = root1.CompareTo(root2);

            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(FileSystemEntryComparisonResult.Removed, actual.Last().Result);
            Assert.AreEqual(@"C:\test\file1.txt", actual.Last().ReferenceFileSystemEntry.Path);
        }

        [TestMethod]
        public void If_1_file_is_moved_comparison_should_return_moved()
        {
            var root1 = FileSystemEntryBuilder.CreateRoot(@"C:\test");

            root1.AddFile("file1.txt");

            var root2 = FileSystemEntryBuilder.CreateRoot(@"C:\test")
                .AddDirAndReturnChild("subfolder")
                .AddFile("file1.txt");

            var actual = root1.CompareTo(root2);

            var file1Comparison = actual.FirstOrDefault(f => f.ReferenceFileSystemEntry.Path == @"C:\test\file1.txt");
            Assert.IsNotNull(file1Comparison);
            Assert.AreEqual(FileSystemEntryComparisonResult.Moved, file1Comparison.Result);
            Assert.AreEqual(@"C:\test\file1.txt", file1Comparison.ReferenceFileSystemEntry.Path);
            Assert.AreEqual(@"C:\test\subfolder\file1.txt", file1Comparison.CurrentFileSystemEntries.Single().Path);
        }

        [TestMethod]
        public void If_1_directory_is_removed_comparison_should_return_removed()
        {
            var root1 = FileSystemEntryBuilder.CreateRoot(@"C:\test")
                .AddDirAndReturnParent("subfolder");

            var root2 = FileSystemEntryBuilder.CreateRoot(@"C:\test")
                .AddFile("file1.txt");

            var actual = root1.CompareTo(root2);

            var subfolderComparison = actual.FirstOrDefault(f => f.ReferenceFileSystemEntry.Path == @"C:\test\subfolder");
            Assert.IsNotNull(subfolderComparison);
            Assert.AreEqual(FileSystemEntryComparisonResult.Removed, subfolderComparison.Result);
            Assert.AreEqual(@"C:\test\subfolder", subfolderComparison.ReferenceFileSystemEntry.Path);
        }
    }
}