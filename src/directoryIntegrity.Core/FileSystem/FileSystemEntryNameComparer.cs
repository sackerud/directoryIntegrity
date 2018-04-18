using System.Collections.Generic;

namespace directoryIntegrity.Core.FileSystem
{
    internal class FileSystemEntryNameComparer : IEqualityComparer<IFileSystemEntry>
    {
        public bool Equals(IFileSystemEntry x, IFileSystemEntry y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(IFileSystemEntry obj)
        {
            return obj == null ? 0 : obj.Path.GetHashCode();
        }
    }
}