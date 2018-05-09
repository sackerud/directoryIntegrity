using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using directoryIntegrity.Core.ReferenceFile;
using directoryIntegrity.Core.Scan;
using Newtonsoft.Json;

namespace directoryIntegrity.ConsoleApp
{
    internal class Program
    {
        private static ScanOptions _scanOptions;
        private static CreateReferenceFileOptions _createRefFileOptions;

        static int Main(string[] args)
        {
            return ConsumeArguments(args);
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
            Console.WriteLine(string.Join(Environment.NewLine, errs.Select(e => e.Tag)));
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

            _scanOptions = opts;
            return 0;
        }
    }
}