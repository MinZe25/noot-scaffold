namespace noot_scaffold.Formats;
using System.Text;

public static partial class Formatter
{
    
    private static class {{name;format=pascal}}
    {
        public static StringBuilder Format(string input)
        {
            var sb = new StringBuilder();
            var values = input.Split(delimiters).ToList();
            return sb;
        }
    }
}