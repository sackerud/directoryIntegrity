using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CommandLine;
using directoryIntegrity.Core;
using directoryIntegrity.Core.FileSystem;
using directoryIntegrity.Core.ReferenceFile;
using directoryIntegrity.Core.Scan;
using Newtonsoft.Json;
using Directory = System.IO.Directory;
using File = System.IO.File;

namespace directoryIntegrity.ConsoleApp
{
    internal class Program
    {
        private static ScanOptions _scanOptions;
        private static CreateReferenceFileOptions _createRefFileOptions;

        static int Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var exitCode = ConsumeArguments(args);
            Console.WriteLine($"This operation took {PrintDuration(sw.Elapsed)}");
            return exitCode;
        }

        private static string PrintDuration(TimeSpan duration)
        {
            var sb = new StringBuilder();

            if (duration.TotalHours >= 1)
                sb.Append($"{duration.TotalHours}h ");

            if (duration.Seconds >= 1)
                sb.Append($"{duration.Seconds}s ");

            sb.Append($"{duration.Milliseconds}ms");

            return sb.ToString();
        }

        private static int ConsumeArguments(string[] args)
        {
            return Parser.Default.ParseArguments<CreateReferenceFileOptions, ScanOptions>(args)
                .MapResult(
                    (CreateReferenceFileOptions opts) => CreateReferenceFile(opts),
                    (ScanOptions opts) => ScanDirectory(opts),
                    HandleParseError);
        }

        private static int HandleParseError(IEnumerable<Error> errs)
        {
            return -1;
        }

        private static int CreateReferenceFile(CreateReferenceFileOptions opts)
        {
            if (!Directory.Exists(opts.DirectoryToScan))
            {
                Console.WriteLine($"{opts.DirectoryToScan} does not exist");
                return -1;
            }

            _createRefFileOptions = opts;

            var scanner = new DirectoryScanner(_createRefFileOptions.DirectoryToScan);

            new JsonReferenceFileCreator(scanner, Formatting.Indented)
                .CreateReferenceFile(_createRefFileOptions.ReferenceFilepath);

            return 0;
        }

        private static int ScanDirectory(ScanOptions opts)
        {
            if (!Directory.Exists(opts.DirectoryToScan))
            {
                Console.WriteLine($"{opts.DirectoryToScan} does not exist");
                return -1;
            }

            if (!File.Exists(opts.ReferenceFile))
            {
                Console.WriteLine($"{opts.ReferenceFile} does not exist");
            }

            _scanOptions = opts;

            var scanner = new DirectoryScanner(_scanOptions.DirectoryToScan);
            var scanResult = scanner.Scan();

            var referenceFileContents = new JsonReferenceFileReader().Read(_scanOptions.ReferenceFile);

            var comparison = scanResult.FirstOrDefault().CompareTo(referenceFileContents.FirstOrDefault());

            PrintComparison(comparison);

            return 0;
        }

        private static void PrintComparison(IEnumerable<FileSystemEntryComparison> comparison)
        {
            Console.WriteLine($@"Intact folders: {comparison.Count(c => c.Result == FileSystemEntryComparisonResult.Intact
                                                                       && c.ReferenceFileSystemEntry.IsDirectory)}");
            Console.WriteLine($@"Intact files: {comparison.Count(c => c.Result == FileSystemEntryComparisonResult.Intact
                                                                        && !c.ReferenceFileSystemEntry.IsDirectory)}");

            Console.WriteLine("The following files and directories has been removed:");

            foreach (var fseComparison in comparison.Where(c => c.Result == FileSystemEntryComparisonResult.Removed))
            {
                Console.WriteLine($"{fseComparison.ReferenceFileSystemEntry.Path}");
            }

            Console.WriteLine("The following files and directories may have been moved:");
            foreach (var fseComparison in comparison.Where(c => c.Result == FileSystemEntryComparisonResult.Moved))
            {
                Console.WriteLine($"{fseComparison.ReferenceFileSystemEntry.Path}");
                foreach (var movedFse in fseComparison.CurrentFileSystemEntries)
                {
                    Console.WriteLine($"\t=>{movedFse.Path}");
                }
            }
        }
    }
}