using System;
using System.IO;

namespace directoryIntegrity.Core.ReferenceFile.Naming
{
    public class ReferenceFilePreserver : IReferenceFileBaptist
    {
        private readonly DateTime _dateToIncludeInFilename;

        public ReferenceFilePreserver(DateTime dateToIncludeInFilename)
        {
            _dateToIncludeInFilename = dateToIncludeInFilename;
        }

        public string Baptise(string filepath)
        {
            var dir = Path.GetDirectoryName(filepath);
            var filename = Path.GetFileNameWithoutExtension(filepath);
            var extension = Path.GetExtension(filepath);

            var dateSuffix = _dateToIncludeInFilename.ToString("yyyy-MM-dd_hh-mm-ss");
            var filenameWithDateSuffix = $"{filename}.{dateSuffix}{extension}";

            return $"{Path.Combine(dir, filenameWithDateSuffix)}";
        }
    }
}