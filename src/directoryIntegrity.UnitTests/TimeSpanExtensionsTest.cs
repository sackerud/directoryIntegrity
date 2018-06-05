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

        [TestMethod]
        public void Onethousand_and_1_ms_should_format_to_1s_1ms()
        {
            var sut = new TimeSpan(0, 0, 0, 0, 1001);

            var actual = sut.Format();

            Assert.AreEqual("1s 1ms", actual);
        }

        [TestMethod]
        public void One_hour_should_format_to_1h()
        {
            var sut = new TimeSpan(0, 1, 0, 0, 0);

            var actual = sut.Format();

            Assert.AreEqual("1h", actual);
        }

        [TestMethod]
        public void Sixtyone_minutes_should_format_to_1h_1m()
        {
            var sut = new TimeSpan(0, 0, 61, 0, 0);

            var actual = sut.Format();

            Assert.AreEqual("1h 1m", actual);
        }

        [TestMethod]
        public void Sixtyone_seconds_should_format_to_1m_1s()
        {
            var sut = new TimeSpan(0, 0, 0, 61, 0);

            var actual = sut.Format();

            Assert.AreEqual("1m 1s", actual);
        }

        [TestMethod]
        public void Twentyfive_hours_should_format_to_1_day_1h()
        {
            var sut = new TimeSpan(0, 25, 0, 0, 0);

            var actual = sut.Format();

            Assert.AreEqual("1 day 1h", actual);
        }

        [TestMethod]
        public void Two_days_should_format_to_2_days()
        {
            var sut = new TimeSpan(2, 0, 0, 0, 0);

            var actual = sut.Format();

            Assert.AreEqual("2 days", actual);
        }
    }
}