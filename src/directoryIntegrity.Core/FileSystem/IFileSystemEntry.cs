using System.Collections.Generic;
using directoryIntegrity.Core.ReferenceFile;
using Newtonsoft.Json;

namespace directoryIntegrity.Core.FileSystem
{
    [JsonConverter(typeof(JsonReferenceFileConverter))]
    public interface IFileSystemEntry
    {
        IList<IFileSystemEntry> Children { get; }
        string Path { get; set; }

        string Name { get; }

        bool IsDirectory { get; }
    }
}