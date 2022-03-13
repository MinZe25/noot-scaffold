using System.Text;
using System;

namespace noot_scaffold.Formats;

public static partial class Formatter
{
    private static class SnakeCaseFormatter
    {
        private static string CharacterToUpper(char c)
        {
            return c.ToString().ToUpper();
        }

        public static StringBuilder Format(string input)
        {
            var values = input.Split(delimiters).ToList();
            return new StringBuilder(string.Join("_", values));
        }
    }
}