using System.Text;

namespace noot_scaffold.Formats;

public interface IFormatter
{
    static char[] delimiters =>
        new[]
        {
            '.', ' ', '_'
        };

    StringBuilder Format(string input);
}