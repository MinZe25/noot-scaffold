using System.Text;
using System;

namespace noot_scaffold.Formats;

public static partial class Formatter
{
    private static class PascalCaseFormatter
    {
        private static string CharacterToUpper(char c)
        {
            return c.ToString().ToUpper();
        }

        public static StringBuilder Format(string input)
        {
            var values = input.Split(delimiters).ToList();
            return new StringBuilder(string.Join("", values.Select(s => $"{CharacterToUpper(s[0])}{s[1..]}")));
        }
    }
}