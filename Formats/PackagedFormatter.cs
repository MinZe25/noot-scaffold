using System.Text;

namespace noot_scaffold.Formats;

public static partial class ScaffoldFormatter
{
    private class PackagedFormatter : IFormatter
    {
        private string CharacterToLower(char c)
        {
            return c.ToString().ToLower();
        }

        public StringBuilder Format(string input)
        {
            string[] values = input.Split(IFormatter.delimiters);
            var sb = new StringBuilder();
            for (var i = 0; i < values.Length; i++)
            {
                string s = values[i];
                var str = new StringBuilder();
                if (i != 0)
                    str.Append('.');
                str.Append($"{CharacterToLower(s[0])}{s[1..]}");
                sb.Append(str);
            }

            return sb;
        }
    }
}