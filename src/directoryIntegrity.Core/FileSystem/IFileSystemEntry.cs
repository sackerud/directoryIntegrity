using System.Collections.Generic;

namespace directoryIntegrity.Core.FileSystem
{
    public interface IFileSystemEntry
    {
        IList<IFileSystemEntry> Children { get; }
        string Path { get; set; }

        string Name { get; }

        bool IsDirectory { get; }
    }
}