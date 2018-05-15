using System;
using directoryIntegrity.Core.ReferenceFile.Naming;

namespace directoryIntegrity.ConsoleApp
{
    internal class ReferenceFileBaptistSelector
    {
        internal static IReferenceFileBaptist SelectBaptist(CreateReferenceFileOptions opts, DateTime? lastModifiedOfRefFile = null)
        {
            if (opts.OverwriteReferenceFile) return new ReferenceFileOverwriter();

            if (!lastModifiedOfRefFile.HasValue)
                throw new ArgumentNullException(nameof(lastModifiedOfRefFile), "A DateTime object is needed when reference file shall be preserved");

            return new ReferenceFilePreserver(lastModifiedOfRefFile.Value);
        }
    }
}