using noot_scaffold.Formats;

namespace noot_scaffold;

public static class ExtensionMethods
{
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (item, index));
    }
}