using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommandLine;
using directoryIntegrity.Core;
using directoryIntegrity.Core.FileSystem;
using directoryIntegrity.Core.Formatters;
using directoryIntegrity.Core.ReferenceFile;
using directoryIntegrity.Core.Scan;
using Newtonsoft.Json;
using Directory = System.IO.Directory;
using File = System.IO.File;

namespace directoryIntegrity.ConsoleApp
{
    internal class Program
    {
        internal static ScanOptions ScanOptions { get; set; }
        internal static CreateReferenceFileOptions CreateRefFileOptions { get; set; }
        internal static bool PreventCreatingReferenceFile { get; set; }
        internal static bool PreventScan { get; set; }

        static int Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var exitCode = ConsumeArguments(args);
            Console.WriteLine($"This operation took {sw.Elapsed.Format()}");
            return exitCode;
        }

        internal static int ConsumeArguments(string[] args)
        {
            return Parser.Default.ParseArguments<CreateReferenceFileOptions, ScanOptions>(args)
                .MapResult(
                    (CreateReferenceFileOptions opts) => EnsureDirectoryToScanExistsAndCreateReferenceFile(opts),
                    (ScanOptions opts) => EnsureDirectoryToScanAndRefFileExistsAndStartScan(opts),
                    HandleParseError);
        }

        private static int HandleParseError(IEnumerable<Error> errs)
        {
            return ExitCodes.InvalidArgs;
        }

        private static int EnsureDirectoryToScanExistsAndCreateReferenceFile(CreateReferenceFileOptions opts)
        {
            if (!Directory.Exists(opts.DirectoryToScan))
            {
                Console.WriteLine($"{opts.DirectoryToScan} does not exist");
                return -1;
            }

            CreateRefFileOptions = opts;

            var scanner = new DirectoryScanner(CreateRefFileOptions.DirectoryToScan);

            CreateReferenceFile(scanner);

            return ExitCodes.Success;
        }

        private static void CreateReferenceFile(DirectoryScanner scanner)
        {
            if (PreventCreatingReferenceFile)
            {
                Console.WriteLine($"Skipping reference file creation due to {nameof(PreventCreatingReferenceFile)} = {PreventCreatingReferenceFile}");
                return;
            }

            new JsonReferenceFileCreator(scanner, Formatting.Indented)
                .CreateReferenceFile(CreateRefFileOptions.ReferenceFilepath);
        }

        private static int EnsureDirectoryToScanAndRefFileExistsAndStartScan(ScanOptions opts)
        {
            if (!Directory.Exists(opts.DirectoryToScan))
            {
                Console.WriteLine($"{opts.DirectoryToScan} does not exist");
                return ExitCodes.ScanDirDoesNotExist;
            }

            if (opts.ReferenceFileIsSpecified && !File.Exists(opts.ReferenceFile))
            {
                Console.WriteLine($"{opts.ReferenceFile} does not exist");
                return ExitCodes.RefFileDoesNotExist;
            }

            ScanOptions = opts;

            var comparison = Scan();

            PrintComparison(comparison.ToList());

            return ExitCodes.Success;
        }

        private static IEnumerable<FileSystemEntryComparison> Scan()
        {
            if (PreventScan)
            {
                Console.WriteLine($"Skipping scan due to {nameof(PreventScan)} = {PreventScan}");
                return new List<FileSystemEntryComparison>();
            }

            var scanner = new DirectoryScanner(ScanOptions.DirectoryToScan);
            var scanResult = scanner.Scan();

            var referenceFileContents = new JsonReferenceFileReader().Read(ScanOptions.ReferenceFile);

            var comparison = referenceFileContents.FirstOrDefault().CompareTo(scanResult.FirstOrDefault());
            return comparison;
        }

        private static void PrintComparison(IList<FileSystemEntryComparison> comparison)
        {
            var comparisonResult = new DirectoryIntegrityResult(comparison);

            Console.WriteLine($@"Intact folders: {comparisonResult.IntactDirectoriesCount}");
            Console.WriteLine($@"Intact files: {comparisonResult.IntactFilesCount}");

            PrintRemoved(comparison);
            PrintMoved(comparison);
        }

        private static void PrintMoved(IList<FileSystemEntryComparison> comparison)
        {
            var movedEntries = comparison.Where(c => c.Result == FileSystemEntryComparisonResult.Moved).ToList();

            if (!movedEntries.Any()) return;

            Console.WriteLine("The following files and directories may have been moved:");
            foreach (var fseComparison in movedEntries)
            {
                Console.WriteLine($"{fseComparison.ReferenceFileSystemEntry.Path}");
                foreach (var movedFse in fseComparison.CurrentFileSystemEntries)
                {
                    Console.WriteLine($"\t=>{movedFse.Path}");
                }
            }
        }

        private static void PrintRemoved(IList<FileSystemEntryComparison> comparison)
        {
            var removedEntries = comparison.Where(c => c.Result == FileSystemEntryComparisonResult.Removed).ToList();

            if (!removedEntries.Any()) return;

            Console.WriteLine("The following files and directories has been removed:");

            foreach (var fseComparison in removedEntries)
            {
                Console.WriteLine($"{fseComparison.ReferenceFileSystemEntry.Path}");
            }
        }
    }
}