namespace noot_scaffold;

public static class ScaffoldDirectoryDetector
{
    private const string ScaffoldFolder = ".scaffold";

    public static bool DetectScaffold(string currentPath)
    {
        return Directory.Exists(Path.Combine(currentPath, ScaffoldFolder));
    }

    public static string GetScaffoldPath(string currentPath)
    {
        return Path.Combine(currentPath, ScaffoldFolder);
    }

    public static List<string> GetAllScaffolds(string currentPath)
    {
        string dir = Path.Combine(currentPath, ScaffoldFolder);
        if (!Directory.Exists(dir))
        {
            throw new DirectoryNotFoundException($"{ScaffoldFolder} directory not found in {currentPath}");
        }

        return Directory.GetDirectories(dir).ToList();
    }

    public static bool TryGetScaffoldPath(string currentPath, string scaffoldName, out string scaffoldPath)
    {
        scaffoldPath = string.Empty;
        if (!DetectScaffold(currentPath)) return false;
        string dir = Path.Combine(GetScaffoldPath(currentPath), scaffoldName);
        if (!Directory.Exists(dir)) return false;
        scaffoldPath = dir;
        return true;
    }

    public static Dictionary<string, string> ReadScaffoldProperties(string scaffoldPath)
    {
        var properties = new Dictionary<string, string>();
        string file = Path.Combine(scaffoldPath, "scaffold.properties");
        if (!File.Exists(file)) return properties;
        string[] lines = File.ReadAllLines(file);
        foreach (string line in lines)
        {
            string[] split = line.Split('=');
            if (split.Length != 2) continue;
            properties.Add(split[0], split[1]);
        }

        return properties;
    }
}