using CommandLine;

namespace directoryIntegrity.ConsoleApp
{
    [Verb("createref", HelpText = "Create a reference file which represents a snapshot of a directory")]
    internal class CreateReferenceFileOptions
    {
        [Option('d', "dirtoscan", Required = true, HelpText = "The directory to create a shapshot of")]
        public string DirectoryToScan { get; set; }

        [Option('o', "outputpath", HelpText = "Location to where the reference file will be stored. Default is the current directory.", Default = @".\")]
        public string ReferenceFilepath { get; set; } = @".\";

        [Option('w', "overwrite", Default = false, Required = false,
            HelpText = "Specifying -o or overwrite==true will overwrite any existing refrence file with the same name")]
        public bool OverwriteReferenceFile { get; set; }
    }

    [Verb("scan", HelpText = "Scan a directory for changes compared to a reference file")]
    internal class ScanOptions
    {
        [Option('r',
                "referencefile",
                Required = false, 
                HelpText = "The reference file used for comparison with the scan of a directory")]
        public string ReferenceFile { get; set; }

        public bool ReferenceFileIsSpecified => !string.IsNullOrWhiteSpace(ReferenceFile);

        [Option('d', "dirtoscan", Required = true, HelpText = "The directory to create a shapshot of")]
        public string DirectoryToScan { get; set; }
    }
}