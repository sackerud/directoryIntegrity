using System.IO;
using directoryIntegrity.Core.DateAndTime;

namespace directoryIntegrity.Core.ReferenceFile.Naming
{
    public class ReferenceFilePreserver : IReferenceFileBaptist
    {
        public string Baptise(string filepath)
        {
            var dir = Path.GetDirectoryName(filepath);
            var filename = Path.GetFileNameWithoutExtension(filepath);
            var extension = Path.GetExtension(filepath);

            var dateSuffix = DateTimeProvider.Now.ToString("yyyy-MM-dd_hh-mm-ss");
            var filenameWithDateSuffix = $"{filename}.{dateSuffix}{extension}";

            return $"{Path.Combine(dir, filenameWithDateSuffix)}";
        }
    }
}