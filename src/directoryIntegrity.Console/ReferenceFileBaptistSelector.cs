using directoryIntegrity.Core.ReferenceFile;
using directoryIntegrity.Core.ReferenceFile.Naming;

namespace directoryIntegrity.ConsoleApp
{
    internal class ReferenceFileBaptistSelector
    {
        internal static IReferenceFileBaptist SelectBaptist(CreateReferenceFileOptions opts)
        {
            if (opts.OverwriteReferenceFile) return new ReferenceFileOverwriter();

            return new ReferenceFilePreserver();
        }
    }
}