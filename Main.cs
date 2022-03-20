using System.Drawing;

namespace noot_scaffold;

public class Main
{
    private readonly string _dir;


    private string SelectScaffold()
    {
        Log.WriteLine("No scaffold specified, please select one of the following:", ConsoleColor.Green);
        List<string> allScaffolds = ScaffoldDirectoryDetector.GetAllScaffolds(this._dir);
        string scaffoldDir = ScaffoldDirectoryDetector.GetScaffoldPath(this._dir);
        IEnumerable<string> relativeScaffolds =
            allScaffolds.Select(s => Path.GetRelativePath(scaffoldDir, s));
        foreach ((string item, int index) in relativeScaffolds.WithIndex())
        {
            Log.WriteLine($"[{index}] {item}", ConsoleColor.Green);
        }

        int ind = Input.ReadInt("\nScaffold selected [0]: ", ConsoleColor.Green);
        return allScaffolds[ind];
    }

    public Main(IReadOnlyCollection<string> args)
    {
        this._dir = Directory.GetCurrentDirectory();
        Start(args);
    }

    private void ParseDefaultDuplicate(IReadOnlyCollection<string> args)
    {
        if (args.Any(a => a.StartsWith("--skip")))
            Scaffold.defaultDuplicateResult = Scaffold.DuplicateResult.Skip;
        else if (args.Any(a => a.StartsWith("--overwrite")))
            Scaffold.defaultDuplicateResult = Scaffold.DuplicateResult.Overwrite;
        else if (args.Any(a => a.StartsWith("--cancel")))
            Scaffold.defaultDuplicateResult = Scaffold.DuplicateResult.Cancel;
    }

    private Dictionary<string, string> ParseDefaultProperties(IReadOnlyCollection<string> args)
    {
        var dict = new Dictionary<string, string>();
        string[] a = { "--skip", "--overwrite", "--cancel" };
        for (var i = 0; i < args.Count; i++)
        {
            string arg = args.ElementAt(i);
            if (!arg.StartsWith("--")) continue;
            if (a.Contains(arg)) continue;
            if (!arg.Contains("="))
            {
                dict.Add(arg[2..], args.ElementAt(i + 1));
                i++;
            }
            else
            {
                string[] split = arg.Split('=');
                dict.Add(split[0][2..], split[1]);
            }
        }

        return dict;
    }

    private void Start(IReadOnlyCollection<string> args)
    {
        string scaffold;
        string scaffoldName = args.FirstOrDefault(string.Empty);

        ParseDefaultDuplicate(args);
        if (Scaffold.defaultDuplicateResult != Scaffold.DuplicateResult.Nothing)
        {
            Log.Write("Default duplicate behaviour set to: ", ConsoleColor.Green);
            Log.WriteLine(Scaffold.defaultDuplicateResult.ToString(), ConsoleColor.Cyan);
        }

        if (string.IsNullOrEmpty(scaffoldName) || scaffoldName.StartsWith("--"))
        {
            scaffold = SelectScaffold();
        }
        else if (ScaffoldDirectoryDetector.TryGetScaffoldPath(
                     this._dir, scaffoldName, out string scaffoldPath))
        {
            scaffold = scaffoldPath;
        }
        else
        {
            Log.WriteLine($"scaffold {scaffoldName} not found", ConsoleColor.Red);
            return;
        }

        Dictionary<string, string> Allproperties = ScaffoldDirectoryDetector.ReadScaffoldProperties(scaffold);
        if (Allproperties.TryGetValue("description", out var desc))
        {
            Log.Write("\nDescription: ", ConsoleColor.Green);
            Log.WriteLine(desc, ConsoleColor.Cyan);
        }

        var forcedProperties = ParseDefaultProperties(args);
        var properties = Allproperties
            .Where(s => s.Key != "description" && !forcedProperties.ContainsKey(s.Key))
            .ToDictionary(s => s.Key, s => s.Value);
        foreach ((string? key, string? value) in properties)
        {
            string? sel = Input.ReadString($"{key} [{value}]:", ConsoleColor.Green);
            if (string.IsNullOrEmpty(sel))
            {
                sel = value;
            }

            properties[key] = sel;
        }

        foreach ((string? key, string? value) in forcedProperties)
        {
            properties[key] = value;
        }

        new Scaffold(properties, scaffold, this._dir, true);
    }
}