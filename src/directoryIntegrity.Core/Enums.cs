namespace directoryIntegrity.Core
{
    public enum FileSystemEntryComparisonResult
    {
        NotInitialized = 0,
        Intact = 1,
        Moved = 2,
        Removed = 3,
        Added = 4
    }
}