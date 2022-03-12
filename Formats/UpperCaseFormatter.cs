using System.Text;

namespace noot_scaffold.Formats;

public static partial class ScaffoldFormatter
{
    private class UpperCaseFormatter : IFormatter
    {
        public StringBuilder Format(string input) => new(input.ToUpper());
    }
}