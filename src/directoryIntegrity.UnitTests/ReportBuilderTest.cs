using System;
using System.Collections.Generic;
using directoryIntegrity.ConsoleApp;
using directoryIntegrity.Core.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace directoryIntegrity.UnitTests
{
    [TestClass]
    public class ReportBuilderTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Building_report_from_null_should_throw_exception()
        {
            // Act
            new ReportBuilder(null);
        }

        [TestMethod]
        public void Building_report_from_empty_comparison_should_return_empty_string()
        {
            // Arrange
            var comparison = new List<FileSystemEntryComparison>();

            // Act
            var actual = new ReportBuilder(comparison).Build();

            // Assert
            Assert.AreEqual(ReportBuilder.EmptyComparisonString, actual.ToString());
        }
    }
}