using directoryIntegrity.ConsoleApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace directoryIntegrity.IntegrationTests
{
    [TestClass]
    public class MailHelperTest
    {
        [TestMethod]
        public void CreateMailConfiguration_binds_recipients_correctly()
        {
            var mailCfg = MailHelper.CreateMailConfiguration();

            Assert.AreEqual(2, mailCfg.Recipients.Length);
            Assert.AreEqual("user1@test.local", mailCfg.Recipients[0]);
            Assert.AreEqual("user2@test.local", mailCfg.Recipients[1]);
        }
    }
}