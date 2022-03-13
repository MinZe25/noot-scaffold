using System.Text;

namespace noot_scaffold.Formats;

public static partial class Formatter
{
    private static class UpperCaseFormatter
    {
        public static StringBuilder Format(string input) => new(input.ToUpper());
    }
}