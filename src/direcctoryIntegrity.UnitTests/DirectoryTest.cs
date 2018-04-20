using directoryIntegrity.Core.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace directoryIntegrity.UnitTests
{
    [TestClass]
    public class DirectoryTest
    {
        [TestMethod]
        public void The_directory_name_should_be_returned()
        {
            // Act
            var actual = new Directory(@"C:\temp");

            // Assert
            Assert.AreEqual("temp", actual.Name);
        }
    }
}