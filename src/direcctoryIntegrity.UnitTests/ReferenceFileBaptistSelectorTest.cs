using directoryIntegrity.ConsoleApp;
using directoryIntegrity.Core.ReferenceFile.Naming;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace directoryIntegrity.UnitTests
{

    [TestClass]
    public class ReferenceFileBaptistSelectorTest
    {
        [TestMethod]
        public void When_OverwriteReferenceFile_is_true_ReferenceFileOverwriter_should_be_used()
        {
            var actual = ReferenceFileBaptistSelector.SelectBaptist(
                            new CreateReferenceFileOptions {OverwriteReferenceFile = true});

            Assert.AreEqual(typeof(ReferenceFileOverwriter), actual.GetType());
        }

        [TestMethod]
        public void When_OverwriteReferenceFile_is_false_ReferenceFilePreserver_should_be_used()
        {
            var actual = ReferenceFileBaptistSelector.SelectBaptist(
                new CreateReferenceFileOptions { OverwriteReferenceFile = false });

            Assert.AreEqual(typeof(ReferenceFilePreserver), actual.GetType());
        }
    }
}