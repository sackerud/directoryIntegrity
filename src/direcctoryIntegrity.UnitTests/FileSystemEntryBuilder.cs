using System.IO;
using directoryIntegrity.Core;

namespace directoryIntegrity.UnitTests
{
    internal static class FileSystemEntryBuilder
    {
        internal static FileSystemEntry CreateRoot(string path)
        {
            return new FileSystemEntry(path);
        }

        internal static FileSystemEntry AddFile(this FileSystemEntry parentDir, string filename)
        {
            parentDir.AddChild(new FileSystemEntry(Path.Combine(parentDir.Path, filename)));
            return parentDir;
        }

        internal static FileSystemEntry AddDirAndReturnParent(this FileSystemEntry parentDir, string dirname)
        {
            parentDir.AddChild(new FileSystemEntry(dirname));
            return parentDir;
        }

        internal static FileSystemEntry AddDirAndReturnChild(this FileSystemEntry parentDir, string dirname)
        {
            var childDir = new FileSystemEntry(dirname);
            parentDir.AddChild(childDir);
            return childDir;
        }
    }
}