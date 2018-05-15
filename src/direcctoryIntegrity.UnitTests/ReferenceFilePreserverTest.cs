using System;
using System.Threading;
using directoryIntegrity.Core.DateAndTime;
using directoryIntegrity.Core.ReferenceFile.Naming;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace directoryIntegrity.UnitTests
{

    [TestClass]
    public class ReferenceFilePreserverTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            DateTimeProvider.Thaw();
            DateTimeProvider.Reset();
        }

        [TestMethod]
        public void Name_of_referenceFile_should_be_suffixed_with_dateTime()
        {
            const string refFile = @"C:\temp\dirref.json";

            DateTimeProvider.Set(new DateTime(2018, 1, 1, 11, 12, 13));

            var actual = new ReferenceFilePreserver().Baptise(refFile);

            Assert.AreEqual(@"C:\temp\dirref.2018-01-01_11-12-13.json", actual);
        }
    }
}