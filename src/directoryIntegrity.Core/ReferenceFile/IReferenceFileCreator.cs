using System.IO;
using Newtonsoft.Json;

namespace directoryIntegrity.Core.ReferenceFile
{
    public interface IReferenceFileCreator
    {
        void CreateReferenceFile(string referenceFileOutputPath);
    }

    public class FlatReferenceFileCreator : IReferenceFileCreator
    {
        private readonly IDirectoryScanner _scanner;

        public FlatReferenceFileCreator(IDirectoryScanner scanner)
        {
            _scanner = scanner;
        }

        public void CreateReferenceFile(string referenceFileOutputPath)
        {
            using (var f = new StreamWriter(referenceFileOutputPath))
            {
                foreach (var fsEntry in _scanner.Scan())
                {
                    f.WriteLine(fsEntry.Path);
                }
            }
        }
    }

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

    public enum ReferenceFileFormatting
    {
        None = 0,
        Pretty = 1
    }
}