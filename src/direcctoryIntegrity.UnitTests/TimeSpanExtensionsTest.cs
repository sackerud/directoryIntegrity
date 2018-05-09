using System;
using directoryIntegrity.Core.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace directoryIntegrity.UnitTests
{
    [TestClass]
    public class TimeSpanExtensionsTest
    {
        [TestMethod]
        public void Fivehundred_ms_should_format_to_500ms()
        {
            var sut = new TimeSpan(0, 0, 0, 0, 500);

            var actual = sut.Format();

            Assert.AreEqual("500ms", actual);
        }
    }
}