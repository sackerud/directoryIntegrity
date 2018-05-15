using directoryIntegrity.Core.ReferenceFile.Naming;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace directoryIntegrity.UnitTests
{

    [TestClass]
    public class ReferenceFileOverwriterTest
    {
        [TestMethod]
        public void ReferenceFileOverwriter_should_return_filename_as_is()
        {
            const string refFile = @"C:\temp\dirref.json";

            var actual = new ReferenceFileOverwriter().Baptise(refFile);

            Assert.AreEqual(refFile, actual);
        }
    }
}