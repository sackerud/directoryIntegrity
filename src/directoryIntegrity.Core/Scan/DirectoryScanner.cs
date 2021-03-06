﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using directoryIntegrity.Core.FileSystem;
using Directory = System.IO.Directory;

namespace directoryIntegrity.Core.Scan
{
    public class DirectoryScanner : IDirectoryScanner
    {
        private readonly IFileSystemEntry _rootEntry;

        public DirectoryScanner(string rootDirectory)
        {
            
            ThrowIfDirectoryDoesNotExist(rootDirectory);
            _rootEntry = new FileSystem.Directory(rootDirectory);
        }

        public IEnumerable<IFileSystemEntry> Scan()
        {
            GetFileSystemEntries(_rootEntry);
            return new List<IFileSystemEntry> {_rootEntry};
        }

        private void GetFileSystemEntries(IFileSystemEntry rootDirectory)
        {
            var dirs = ListDirectories(rootDirectory);

            foreach (var dir in dirs)
                rootDirectory.Children.Add(CreateDir(dir));

            foreach (var file in Directory.GetFiles(rootDirectory.Path))
                rootDirectory.Children.Add(CreateFile(file));

            foreach (var dir in dirs)
                GetFileSystemEntries(rootDirectory.Children.Single(c => c.Path == dir));
        }

        private static string[] ListDirectories(IFileSystemEntry rootDirectory)
        {
            return Directory.GetDirectories(rootDirectory.Path);
        }

        private static FileSystem.Directory CreateDir(string dir)
        {
            return new FileSystem.Directory(dir);
        }

        private static FileSystemEntry CreateFile(string dir)
        {
            return new FileSystem.File(dir);
        }

        private void ThrowIfDirectoryDoesNotExist(string directory)
        {
            if (!Directory.Exists(directory)) throw new IOException($"{directory} does not exist");
        }
    }
}