using System;
using System.IO;
using directoryIntegrity.ConsoleApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace directoryIntegrity.UnitTests
{

    [TestClass]
    public class OptionsTest
    {

        [TestMethod]
        public void Createref_verb_without_any_option_should_return_invalidArgs()
        {
            var actual = Program.ConsumeArguments(new []{"createref"});
            Assert.AreEqual(ExitCodes.InvalidArgs, actual);
        }

        [TestMethod]
        public void CreateRef_without_o_option_should_set_overwriteReferenceFile_to_false()
        {
            var dirToScan = Path.GetTempPath();
            var outputPath = Path.Combine(dirToScan, "dirref.json");

            var actual = Program.ConsumeArguments(new[] {"createref", "-d", dirToScan, "-r", outputPath, "-w"});

            Assert.AreEqual(ExitCodes.Success, actual);
            Assert.AreEqual(false, Program.CreateRefFileOptions.OverwriteReferenceFile);
        }

        [TestMethod]
        public void CreateRef_with_o_specified_should_set_overwriteReferenceFile_to_true()
        {
            var dirToScan = Path.GetTempPath();
            var outputPath = Path.Combine(dirToScan, "dirref.json");

            var actual = Program.ConsumeArguments(new[] { "createref", "-d", dirToScan, "-r", outputPath, "-o", "-w" });

            Assert.AreEqual(ExitCodes.Success, actual);
            Assert.AreEqual(true, Program.CreateRefFileOptions.OverwriteReferenceFile);
        }

        [TestMethod]
        public void Createref_verb_with_d_and_w_option_should_not_create_reference_file()
        {
            var dirToScan = Path.GetTempPath();
            var outputPath = Path.Combine(dirToScan, $"{Guid.NewGuid()}.json");

            if (File.Exists(outputPath))
                Assert.Inconclusive($"{outputPath} already exist, cannot make assertions");

            Program.PreventCreatingReferenceFile = false;

            var actual = Program.ConsumeArguments(new[] { "createref", "-w", "-d", dirToScan, "-r", outputPath });
            Assert.AreEqual(ExitCodes.Success, actual);
            Assert.IsFalse(File.Exists(outputPath), $"The -w argument should prevent that {outputPath} is written to disk");
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
            var dirToScan = Path.GetTempPath();
            var actual = Program.ConsumeArguments(new[] { "scan", "-d", dirToScan, "-w" });
            Assert.AreEqual(ExitCodes.Success, actual);
        }

        [TestMethod]
        public void Scan_verb_with_w_option_should_return_success()
        {
            var dirToScan = Path.GetTempPath();
            var outputPath = Path.Combine(dirToScan, $"{Guid.NewGuid()}.json");

            var actual = Program.ConsumeArguments(new[] { "scan", "-w", "-d", dirToScan, "-r", outputPath });
            Assert.AreEqual(ExitCodes.Success, actual);
        }
    }
}