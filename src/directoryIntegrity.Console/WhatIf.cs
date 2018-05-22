using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace directoryIntegrity.ConsoleApp
{
    public static class WhatIf
    {
        internal static void IWouldCreateAReferenceFile(CreateReferenceFileOptions opts)
        {
            var fsEntries = EnumerateFsEntries(opts.DirectoryToScan);
            Console.WriteLine("Here's what I would do:");
            Console.WriteLine($"I would scan {opts.DirectoryToScan} which has {fsEntries.Count()} files and directories");
            Console.WriteLine($"After that, a file with these file system entries would be written to {opts.ReferenceFilepath}");
            if (opts.OverwriteReferenceFile)
                Console.WriteLine("If that file already exists, it will be overwritten");
            else
                Console.WriteLine("If that file already exists, it will be renamed to prevent overwriting");
        }

        internal static void IWouldScan(ScanOptions opts)
        {
            var fsEntries = EnumerateFsEntries(opts.DirectoryToScan);
            Console.WriteLine("Here's what I would do:");
            Console.WriteLine($"I would scan {opts.DirectoryToScan} which has {fsEntries.Count()} files and directories");
            Console.WriteLine($"After that, I'll compare the scan result with {opts.ReferenceFile}");
        }

        private static IEnumerable<string> EnumerateFsEntries(string dirToScan)
        {
            Console.WriteLine("Counting files and directories...");
            var fsEntries = Directory.EnumerateFileSystemEntries(dirToScan, "*.*", SearchOption.AllDirectories);
            return fsEntries;
        }
    }
}
