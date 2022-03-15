using System.Text.RegularExpressions;

namespace noot_scaffold.Formats;

using System.Text;

public static partial class Formatter
{
    private static class PickAt
    {
        private static readonly Regex Rx = new(@"(?<=\().*?(?=\))", RegexOptions.Compiled);

        private static int GetAt(string formatter)
        {
            MatchCollection matches = Rx.Matches(formatter);
            if (matches.Count <= 0) return -1;
            if (int.TryParse(matches.First().Value, out int val)) return val;
            return -1;
        }

        public static StringBuilder Format(string input, string formatter)
        {
            var sb = new StringBuilder();
            int at = GetAt(formatter);
            if (at < 0) return sb.Append(input);
            string[] values = input.Split(delimiters);
            return sb.Append(at >= values.Length ? values[0] : values[at]);
        }
    }
}