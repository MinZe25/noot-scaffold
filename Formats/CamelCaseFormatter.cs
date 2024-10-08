﻿using System.Text;
using System;

namespace noot_scaffold.Formats;

public static partial class Formatter
{
    private static class CamelCaseFormatter
    {
        private static string CharacterToUpper(char c)
        {
            return c.ToString().ToUpper();
        }

        public static StringBuilder Format(string input)
        {
            var sb = new StringBuilder();
            var values = input.Split(delimiters).ToList();
            for (var i = 0; i < values.Count; i++)
            {
                sb.Append(i == 0 ? values[i] : string.Concat(CharacterToUpper(values[i][0]), values[i].AsSpan(1)));
            }

            return sb;
        }
    }
}