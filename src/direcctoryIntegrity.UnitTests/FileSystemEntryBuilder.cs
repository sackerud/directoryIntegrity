using System.IO;
using directoryIntegrity.Core.FileSystem;

namespace directoryIntegrity.UnitTests
{
    internal static class FileSystemEntryBuilder
    {
        internal static FileSystemEntry CreateRoot(string path)
        {
            return new Core.FileSystem.Directory(path);
        }

        internal static FileSystemEntry AddFile(this FileSystemEntry parentDir, string filename)
        {
            parentDir.AddChild(new Core.FileSystem.File(Path.Combine(parentDir.Path, filename)));
            return parentDir;
        }

        internal static FileSystemEntry AddDir(this FileSystemEntry parentDir, string dirname)
        {
            parentDir.AddChild(new Core.FileSystem.Directory(Path.Combine(parentDir.Path, dirname)));
            return parentDir;
        }

        internal static FileSystemEntry AddDirAndReturnParent(this FileSystemEntry parentDir, string dirname)
        {
            parentDir.AddChild(new Core.FileSystem.Directory(Path.Combine(parentDir.Path, dirname)));
            return parentDir;
        }

        internal static FileSystemEntry AddDirAndReturnChild(this FileSystemEntry parentDir, string dirname)
        {
            var childDir = new Core.FileSystem.Directory(Path.Combine(parentDir.Path, dirname));
            parentDir.AddChild(childDir);
            return childDir;
        }
    }
}