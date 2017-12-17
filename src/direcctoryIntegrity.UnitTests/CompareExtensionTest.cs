using System.Collections.Generic;
using System.Linq;
using directoryIntegrity.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace direcctoryIntegrity.UnitTests
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
    }
}