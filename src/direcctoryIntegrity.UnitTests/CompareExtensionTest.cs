using System.Collections.Generic;
using System.Linq;
using directoryIntegrity.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace directoryIntegrity.UnitTests
{
    [TestClass]
    public class CompareExtensionTest
    {
        private IEnumerable<FileSystemEntry> _referenceEntries;

        [TestInitialize]
        public void TestInitialize()
        {
            var rootDir = new FileSystemEntry("rootDir");
            rootDir.AddChild(new FileSystemEntry("fileAtRootDir"));

            var subDir1 = new FileSystemEntry("subDir1");
            rootDir.AddChild(subDir1);

            _referenceEntries = new List<FileSystemEntry>
            {
                rootDir
            };
        }

        [TestMethod]
        public void Comparing_identical_filesystem_structures_should_return_only_intact_result()
        {
            // Act
            var actual = _referenceEntries.CompareTo(_referenceEntries);

            // Assert
            Assert.IsTrue(actual.All(a => a.Result == FileSystemEntryComparisonResult.Intact));
        }

        [TestMethod]
        public void If_1_file_is_missing_comparison_should_return_removed()
        {
            var root1 = FileSystemEntryBuilder.CreateRoot(@"C:\test");

            root1.AddFile("file1.txt");

            var root2 = FileSystemEntryBuilder.CreateRoot(@"C:\test");

            var actual = root1.CompareTo(root2);

            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(FileSystemEntryComparisonResult.Removed, actual.First().Result);
        }
    }
}