using System.Text;

namespace noot_scaffold.Formats;

public static partial class ScaffoldFormatter
{
    private static readonly CamelCaseFormatter CamelCase = new CamelCaseFormatter();
    private static readonly UpperCaseFormatter UpperCase = new UpperCaseFormatter();
    private static readonly PackagedFormatter Packaged = new PackagedFormatter();

    private static StringBuilder Format(StringBuilder input, string format)
    {
        return format switch
        {
            "camel" => CamelCase.Format(input.ToString()),
            "packaged" => Packaged.Format(input.ToString()),
            "upper" => UpperCase.Format(input.ToString()),
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