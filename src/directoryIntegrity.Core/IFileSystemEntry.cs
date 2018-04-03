using System.Collections.Generic;

namespace directoryIntegrity.Core
{
    public interface IFileSystemEntry
    {
        IList<IFileSystemEntry> Children { get; }
        string Path { get; set; }
    }
}