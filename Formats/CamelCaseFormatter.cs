using System.Text;

namespace noot_scaffold.Formats;

public static partial class ScaffoldFormatter
{
    private class CamelCaseFormatter : IFormatter
    {
        private string CharacterToUpper(char c)
        {
            return c.ToString().ToUpper();
        }

        public StringBuilder Format(string input)
        {
            var sb = new StringBuilder();
            var values = input.Split(IFormatter.delimiters).ToList();
            return new StringBuilder(string.Join("", values.Select(s => $"{CharacterToUpper(s[0])}{s[1..]}")));
        }
    }
}