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
            Log.WriteLine($"Selected scaffold: {scaffold}");
        }
        else if (ScaffoldDirectoryDetector.TryGetScaffoldPath(
                     this._dir, scaffoldName, out string scaffoldPath))
        {
            scaffold = scaffoldPath;
            Log.WriteLine($"Selected scaffold: {scaffold}");
        }
        else
        {
            Log.WriteLine($"scaffold {scaffoldName} not found", ConsoleColor.Red);
            return;
        }

        Dictionary<string, string> properties = ScaffoldDirectoryDetector.ReadScaffoldProperties(scaffold);
        foreach ((string? key, string? value) in properties)
        {
            var sel = Input.ReadString($"{key}[{value}]=", ConsoleColor.Green);
            if (string.IsNullOrEmpty(sel))
            {
                sel = value;
            }

            Log.WriteLine($"{key}={sel}");
        }
    }
}