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
        private static ScanOptions _scanOptions;
        private static CreateReferenceFileOptions _createRefFileOptions;

        static int Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var exitCode = ConsumeArguments(args);
            Console.WriteLine($"This operation took {sw.Elapsed.Format()}");
            return exitCode;
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
            return ExitCodes.INVALID_ARGS;
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

            return ExitCodes.SUCCESS;
        }

        private static int ScanDirectory(ScanOptions opts)
        {
            if (!Directory.Exists(opts.DirectoryToScan))
            {
                Console.WriteLine($"{opts.DirectoryToScan} does not exist");
                return ExitCodes.SCAN_DIR_DOES_NOT_EXIST;
            }

            if (opts.ReferenceFileIsSpecified && !File.Exists(opts.ReferenceFile))
            {
                Console.WriteLine($"{opts.ReferenceFile} does not exist");
                return ExitCodes.REF_FILE_DOES_NOT_EXIST;
            }

            _scanOptions = opts;

            var scanner = new DirectoryScanner(_scanOptions.DirectoryToScan);
            var scanResult = scanner.Scan();

            var referenceFileContents = new JsonReferenceFileReader().Read(_scanOptions.ReferenceFile);

            var comparison = scanResult.FirstOrDefault().CompareTo(referenceFileContents.FirstOrDefault());

            PrintComparison(comparison);

            return ExitCodes.SUCCESS;
        }

        private static void PrintComparison(IEnumerable<FileSystemEntryComparison> comparison)
        {
            var comparisonResult = new DirectoryIntegrityResult(comparison);

            Console.WriteLine($@"Intact folders: {comparisonResult.IntactDirectoriesCount}");
            Console.WriteLine($@"Intact files: {comparisonResult.IntactFilesCount}");

            PrintRemoved(comparison);
            PrintMoved(comparison);
        }

        private static void PrintMoved(IEnumerable<FileSystemEntryComparison> comparison)
        {
            var movedEntries = comparison.Where(c => c.Result == FileSystemEntryComparisonResult.Moved);

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

        private static void PrintRemoved(IEnumerable<FileSystemEntryComparison> comparison)
        {
            var removedEntries = comparison.Where(c => c.Result == FileSystemEntryComparisonResult.Removed);

            if (!removedEntries.Any()) return;

            Console.WriteLine("The following files and directories has been removed:");

            foreach (var fseComparison in removedEntries)
            {
                Console.WriteLine($"{fseComparison.ReferenceFileSystemEntry.Path}");
            }
        }
    }

    public class ExitCodes
    {
        public const int REF_FILE_DOES_NOT_EXIST = 400;
        public const int SCAN_DIR_DOES_NOT_EXIST = 404;
        public const int INVALID_ARGS = 501;

        public const int SUCCESS = 0;
    }
}