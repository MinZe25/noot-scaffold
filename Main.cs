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
            Log.WriteLine($"[{index}]{item}", ConsoleColor.Green);
        }

        int ind = Input.ReadInt("Selection[0]: ", ConsoleColor.Green);
        return allScaffolds[ind];
    }

    public Main() : this(Array.Empty<string>())
    {
    }

    public Main(IReadOnlyCollection<string> args)
    {
        this._dir = Directory.GetCurrentDirectory();
        Start(args);
    }

    private void Start(IReadOnlyCollection<string> args)
    {
        string scaffold;
        string scaffoldName = args.FirstOrDefault(string.Empty);
        if (string.IsNullOrEmpty(scaffoldName))
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
            Log.WriteLine(desc, ConsoleColor.Cyan);
        }

        var properties = Allproperties
            .Where(s => s.Key != "description")
            .ToDictionary(s => s.Key, s => s.Value);
        foreach ((string? key, string? value) in properties)
        {
            string? sel = Input.ReadString($"{key}[{value}]=", ConsoleColor.Green);
            if (string.IsNullOrEmpty(sel))
            {
                sel = value;
            }

            properties[key] = sel;
            Log.WriteLine($"{key}={sel}");
        }

        new Scaffold(properties, scaffold, this._dir, true);
    }
}