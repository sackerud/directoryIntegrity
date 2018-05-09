using System;
using System.Text;

namespace directoryIntegrity.Core.Formatters
{
    public static class TimeSpanExtensions
    {
        public static string Format(this TimeSpan duration)
        {
            var sb = new StringBuilder();

            if (duration.TotalHours >= 1)
                sb.Append($"{duration.TotalHours}h ");

            if (duration.Seconds >= 1)
                sb.Append($"{duration.Seconds}s ");

            sb.Append($"{duration.Milliseconds}ms");

            return sb.ToString();
        }
    }
}