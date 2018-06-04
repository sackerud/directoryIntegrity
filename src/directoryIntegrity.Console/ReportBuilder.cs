using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using directoryIntegrity.Core;
using directoryIntegrity.Core.FileSystem;

namespace directoryIntegrity.ConsoleApp
{
    public class ReportBuilder
    {
        public const string EmptyComparisonString = "The comparison contains no files or directories";
        private readonly IList<FileSystemEntryComparison> _comparison;

        public ReportBuilder(IEnumerable<FileSystemEntryComparison> comparison)
        {
            if (comparison == null) throw new ArgumentNullException(nameof(comparison));

            _comparison = comparison.ToList();
        }

        public StringBuilder Build()
        {
            var sb = new StringBuilder(PrintComparison(_comparison));
            return sb;
        }

        private string PrintComparison(IList<FileSystemEntryComparison> comparison)
        {
            if (!comparison.Any()) return EmptyComparisonString;

            var comparisonResult = new DirectoryIntegrityResult(comparison);

            var sb = new StringBuilder();

            sb.AppendLine($@"Intact folders: {comparisonResult.IntactDirectoriesCount}");
            sb.AppendLine($@"Intact files: {comparisonResult.IntactFilesCount}");

            sb.AppendLine(PrintRemoved(comparison));
            sb.AppendLine(PrintMoved(comparison));
            sb.AppendLine(PrintAdded(comparison));
            sb.AppendLine();
            sb.AppendLine(PrintComputerInfo());

            return sb.ToString();
        }

        private string PrintMoved(IList<FileSystemEntryComparison> comparison)
        {
            var movedEntries = comparison.Where(c => c.Result == FileSystemEntryComparisonResult.Moved).ToList();

            if (!movedEntries.Any()) return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine("The following files and directories may have been moved:");
            foreach (var fseComparison in movedEntries)
            {
                sb.AppendLine($"{fseComparison.ReferenceFileSystemEntry.Path}");
                foreach (var movedFse in fseComparison.CurrentFileSystemEntries)
                {
                    sb.AppendLine($"\t=>{movedFse.Path}");
                }
            }

            return sb.ToString();
        }

        private string PrintAdded(IList<FileSystemEntryComparison> comparison)
        {
            var addedEntries = comparison.Where(c => c.Result == FileSystemEntryComparisonResult.Added).ToList();

            if (!addedEntries.Any()) return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine("The following files and directories has been added:");

            foreach (var fseComparison in addedEntries)
            {
                sb.AppendLine(fseComparison.CurrentFileSystemEntries.Single().Path);
            }

            return sb.ToString();
        }

        private string PrintRemoved(IList<FileSystemEntryComparison> comparison)
        {
            var removedEntries = comparison.Where(c => c.Result == FileSystemEntryComparisonResult.Removed).ToList();

            if (!removedEntries.Any()) return string.Empty;

            var sb = new StringBuilder();

            sb.AppendLine("The following files and directories has been removed:");

            foreach (var fseComparison in removedEntries)
            {
                sb.AppendLine($"{fseComparison.ReferenceFileSystemEntry.Path}");
            }

            return sb.ToString();
        }

        private static string PrintComputerInfo()
        {
            return $"This job ran on {Environment.MachineName}";
        }
    }
}