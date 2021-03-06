﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CommandLine;
using directoryIntegrity.Core;
using directoryIntegrity.Core.FileSystem;
using directoryIntegrity.Core.Formatters;
using directoryIntegrity.Core.ReferenceFile;
using directoryIntegrity.Core.ReferenceFile.Naming;
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
                return ExitCodes.ScanDirDoesNotExist;
            }

            CreateRefFileOptions = opts;

            var scanner = new DirectoryScanner(CreateRefFileOptions.DirectoryToScan);

            CreateReferenceFile(scanner);

            return ExitCodes.Success;
        }

        private static void CreateReferenceFile(IDirectoryScanner scanner)
        {
            if (CreateRefFileOptions.WhatIf)
            {
                Console.WriteLine("Skipping reference file creation due to --whatif argument");
                WhatIf.IWouldCreateAReferenceFile(CreateRefFileOptions);
                return;
            }

            MaybePreserveOldeReferenceFile();

            new JsonReferenceFileCreator(scanner, Formatting.Indented)
                .CreateReferenceFile(CreateRefFileOptions.ReferenceFilepath);
        }

        private static void MaybePreserveOldeReferenceFile()
        {
            if (!File.Exists(CreateRefFileOptions.ReferenceFilepath)) return;

            if (CreateRefFileOptions.OverwriteReferenceFile) return;

            var lastWriteTime = File.GetLastWriteTime(CreateRefFileOptions.ReferenceFilepath);

            var baptist = new ReferenceFilePreserver(lastWriteTime);

            File.Move(CreateRefFileOptions.ReferenceFilepath, baptist.Baptise(CreateRefFileOptions.ReferenceFilepath));
        }

        private static int EnsureDirectoryToScanAndRefFileExistsAndStartScan(ScanOptions opts)
        {
            if (opts.WhatIf)
            {
                Console.WriteLine("Skipping scan due to --whatif argument");
                WhatIf.IWouldScan(opts);
                return ExitCodes.Success;
            }

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

            var report = new ReportBuilder(comparison).Build();

            MailHelper.SendReportByMailDontThrow(report.ToString(), opts);

            return ExitCodes.Success;
        }

        private static IEnumerable<FileSystemEntryComparison> Scan()
        {
            var scanner = new DirectoryScanner(ScanOptions.DirectoryToScan);
            var scanResult = scanner.Scan();

            var referenceFileContents = new JsonReferenceFileReader().Read(ScanOptions.ReferenceFile);

            var comparison = referenceFileContents.FirstOrDefault().CompareTo(scanResult.FirstOrDefault());
            return comparison;
        }
    }
}