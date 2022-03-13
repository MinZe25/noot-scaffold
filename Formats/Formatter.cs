using System.Text;

namespace noot_scaffold.Formats;

public static partial class Formatter
{
    private static char[] delimiters =>
        new[]
        {
            '.', ' ', '_'
        };


    private static StringBuilder Format(StringBuilder input, string format)
    {
        return format switch
        {
            "camel" => CamelCaseFormatter.Format(input.ToString()),
            "package" => PackagedFormatter.Format(input.ToString()),
            "upper" => UpperCaseFormatter.Format(input.ToString()),
            "lower" => LowerCaseFormatter.Format(input.ToString()),
            "snake" => SnakeCaseFormatter.Format(input.ToString()),
            "kebab" => KebabCaseFormatter.Format(input.ToString()),
            "pascal" => PascalCaseFormatter.Format(input.ToString()),
            _ => input
        };
    }

    public static string MultipleFormat(string input, string formats)
    {
        var sb = new StringBuilder(input);
        sb = formats.Split(",").Aggregate(sb, Format);

        return sb.ToString();
    }
}