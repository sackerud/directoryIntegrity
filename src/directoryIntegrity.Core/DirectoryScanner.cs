using System.Collections.Generic;
using System.IO;
using directoryIntegrity.Core.FileSystem;
using Directory = System.IO.Directory;

namespace directoryIntegrity.Core
{
    public class DirectoryScanner : IDirectoryScanner
    {
        private readonly string _rootDirectory;
        private bool _isAtRootLevel = true;

        public DirectoryScanner(string rootDirectory)
        {
            _rootDirectory = rootDirectory;
            ThrowIfDirectoryDoesNotExist(rootDirectory);
        }

        public IEnumerable<IFileSystemEntry> Scan()
        {
            return GetFileSystemEntries(_rootDirectory);
        }

        private IEnumerable<IFileSystemEntry> GetFileSystemEntries(string rootDirectory)
        {
            if (_isAtRootLevel)
            {
                _isAtRootLevel = false;
            }
            else
            {
                yield return new FileSystem.Directory(rootDirectory);
            }

            foreach (var file in Directory.GetFiles(rootDirectory))
                yield return new FileSystem.File(file);

            foreach (var dir in Directory.GetDirectories(rootDirectory))
            foreach (var file in GetFileSystemEntries(dir))
                yield return new FileSystem.File(file.Path);
        }

        private void ThrowIfDirectoryDoesNotExist(string directory)
        {
            if (!Directory.Exists(directory)) throw new IOException($"{directory} does not exist");
        }
    }
}