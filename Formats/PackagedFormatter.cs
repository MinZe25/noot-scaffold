﻿using System.Text;

namespace noot_scaffold.Formats;

public static partial class Formatter
{
    private static class PackagedFormatter
    {
        private static string CharacterToLower(char c)
        {
            return c.ToString().ToLower();
        }

        public static StringBuilder Format(string input)
        {
            string[] values = input.Split(delimiters);
            var sb = new StringBuilder();
            for (var i = 0; i < values.Length; i++)
            {
                string s = values[i];
                var str = new StringBuilder();
                if (i != 0)
                    str.Append('.');
                str.Append(s);
                sb.Append(str);
            }

            return sb;
        }
    }
}