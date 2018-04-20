namespace directoryIntegrity.Core.ReferenceFile
{
    public interface IReferenceFileCreator
    {
        void CreateReferenceFile(string referenceFileOutputPath);
    }

    public enum ReferenceFileFormatting
    {
        None = 0,
        Pretty = 1
    }
}