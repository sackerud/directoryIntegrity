﻿using System;
using System.IO;
using System.Linq;
using directoryIntegrity.Core;
using directoryIntegrity.Core.ReferenceFile;
using directoryIntegrity.Core.Scan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace directoryIntegrity.UnitTests
{
    [TestClass]
    [Ignore("This should be moved to a separate integration test project")]
    public class FlatFileReferenceCreatorTest
    {
        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void CreateReferenceFile_that_points_to_non_existing_dir_should_throw()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new StreamingDirectoryScanner(Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void CreateReferenceFile_should_create_file_on_disk()
        {
            // Arrange
            var scanner = new StreamingDirectoryScanner(@"C:\temp");

            // Act
            new FlatReferenceFileCreator(scanner).CreateReferenceFile(@"C:\temp\dirref.txt");

            // Assert
            Assert.IsTrue(File.Exists(@"C:\temp\dirref.txt"));
        }

        [TestMethod]
        public void CreateJsonReferenceFile_should_create_file_on_disk()
        {
            var scanner = new DirectoryScanner(@"C:\temp");
            var referenceFileOutputPath = @"C:\temp\dirref.json";
            new JsonReferenceFileCreator(scanner, Formatting.Indented).CreateReferenceFile(referenceFileOutputPath);
            Assert.IsTrue(File.Exists(referenceFileOutputPath));

            var actual = referenceFileOutputPath.Deserialize();
            Assert.IsTrue(actual.Any());
        }
    }
}