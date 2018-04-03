using System.IO;
using directoryIntegrity.Core.Scan;

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
}