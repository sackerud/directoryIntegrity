using System.IO;
using directoryIntegrity.ConsoleApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace directoryIntegrity.UnitTests
{

    [TestClass]
    public class OptionsTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Program.PreventCreatingReferenceFile = true;
            Program.PreventScan = true;
        }

        [TestMethod]
        public void Createref_verb_without_any_option_should_return_invalidArgs()
        {
            var actual = Program.ConsumeArguments(new []{"createref"});
            Assert.AreEqual(ExitCodes.InvalidArgs, actual);
        }

        [TestMethod]
        public void CreateRef_without_w_option_should_set_overwriteReferenceFile_to_false()
        {
            var dirToScan = Path.GetTempPath();
            var outputPath = Path.Combine(dirToScan, "dirref.json");

            var actual = Program.ConsumeArguments(new[] {"createref", "-d", dirToScan, "-o", outputPath});

            Assert.AreEqual(ExitCodes.Success, actual);
            Assert.AreEqual(false, Program.CreateRefFileOptions.OverwriteReferenceFile);
        }

        [TestMethod]
        public void CreateRef_with_w_specified_should_set_overwriteReferenceFile_to_true()
        {
            var dirToScan = Path.GetTempPath();
            var outputPath = Path.Combine(dirToScan, "dirref.json");

            var actual = Program.ConsumeArguments(new[] { "createref", "-d", dirToScan, "-o", outputPath, "-w" });

            Assert.AreEqual(ExitCodes.Success, actual);
            Assert.AreEqual(true, Program.CreateRefFileOptions.OverwriteReferenceFile);
        }

        [TestMethod]
        public void Scan_verb_without_any_option_should_return_invalidArgs()
        {
            var actual = Program.ConsumeArguments(new[] { "scan" });
            Assert.AreEqual(ExitCodes.InvalidArgs, actual);
        }

        [TestMethod]
        public void Scan_verb_without_d_option_should_return_invalidArgs()
        {
            var actual = Program.ConsumeArguments(new[] { "scan", "-r", @"c:\shouldNotMatter.json" });
            Assert.AreEqual(ExitCodes.InvalidArgs, actual);
        }

        [TestMethod]
        public void Scan_verb_with_d_option_should_return_success()
        {
            var actual = Program.ConsumeArguments(new[] { "scan", "-d", @"c:\" });
            Assert.AreEqual(ExitCodes.Success, actual);
        }
    }
}