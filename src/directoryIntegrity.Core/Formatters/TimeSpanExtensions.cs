using System;
using System.Text;

namespace directoryIntegrity.Core.Formatters
{
    public static class TimeSpanExtensions
    {
        public static string Format(this TimeSpan duration)
        {
            var sb = new StringBuilder();

            if (duration.Days == 1)
                sb.Append($"{duration.Days} day ");

            if (duration.Days > 1)
                sb.Append($"{duration.Days} days ");

            if (duration.Hours >= 1)
                sb.Append($"{duration.Hours}h ");

            if (duration.Minutes > 0)
                sb.Append($"{duration.Minutes}m ");

            if (duration.Seconds >= 1)
                sb.Append($"{duration.Seconds}s ");

            if (duration.Milliseconds > 0)
                sb.Append($"{duration.Milliseconds}ms");

            return sb.ToString().TrimEnd();
        }
    }
}