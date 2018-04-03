using System.Collections.Generic;
using directoryIntegrity.Core.FileSystem;

namespace directoryIntegrity.Core.Scan
{
    public interface IDirectoryScanner
    {
        IEnumerable<IFileSystemEntry> Scan();
    }
}