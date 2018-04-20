using System.IO;
using directoryIntegrity.Core.Scan;
using Newtonsoft.Json;

namespace directoryIntegrity.Core.ReferenceFile
{
    public class JsonReferenceFileCreator : IReferenceFileCreator
    {
        private readonly IDirectoryScanner _scanner;
        private readonly Formatting _formatting;

        public JsonReferenceFileCreator(IDirectoryScanner scanner, Formatting formatting = Formatting.None)
        {
            _scanner = scanner;
            _formatting = formatting;
        }

        public void CreateReferenceFile(string referenceFileOutputPath)
        {
            var result = _scanner.Scan();

            var jsonObject = JsonConvert.SerializeObject(result, _formatting);
            File.WriteAllText(referenceFileOutputPath, jsonObject);
        }
    }
}