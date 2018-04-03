using System.Collections.Generic;
using directoryIntegrity.Core.FileSystem;

namespace directoryIntegrity.Core
{
    public interface IDirectoryScanner
    {
        IEnumerable<IFileSystemEntry> Scan();
    }
}