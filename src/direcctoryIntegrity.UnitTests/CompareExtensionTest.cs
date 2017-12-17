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
            _referenceEntries = new List<FileSystemEntry>
            {
                new FileSystemEntry("apa")
            };
        }

        [TestMethod]
        public void Comparing_identical_filesystem_structures_should_return_only_intact_result()
        {
            // Arrange

            // Act
            var actual = _referenceEntries.CompareTo(_referenceEntries);

            // Assert
            Assert.IsTrue(actual.All(a => a.Result == FileSystemEntryComparisonResult.Intact));
        }
    }
}