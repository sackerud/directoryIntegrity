using System.Collections.Generic;
using directoryIntegrity.Core.FileSystem;
using Newtonsoft.Json;

namespace directoryIntegrity.Core.ReferenceFile
{
    public class JsonReferenceFileReader
    {
        public IEnumerable<IFileSystemEntry> Read(string pathToReferenceFile)
        {
            var stringFromFile = System.IO.File.ReadAllText(pathToReferenceFile);

            var result = JsonConvert.DeserializeObject<IEnumerable<IFileSystemEntry>>(stringFromFile);

            return result;
        }
    }
}