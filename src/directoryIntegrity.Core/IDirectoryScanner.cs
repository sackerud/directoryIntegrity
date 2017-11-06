using System.Collections.Generic;

namespace directoryIntegrity.Core
{
    public interface IDirectoryScanner
    {
        IEnumerable<IFileSystemEntry> Scan();
    }
}