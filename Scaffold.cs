using System.Text;
using System.Text.RegularExpressions;
using noot_scaffold.Exceptions;
using noot_scaffold.Formats;

namespace noot_scaffold;

public class Scaffold
{
    public enum DuplicateResult
    {
        Overwrite,
        Skip,
        Cancel
    }

    private Dictionary<string, string> _properties;
    private string _currentDir;
    private string _outputDir;
    private bool skipPropertiesFile;
    private Regex _rx = new("{{(?<input>.*?)}}", RegexOptions.Compiled);

    public Scaffold(Dictionary<string, string> properties, string currentDir, string outputDir,
        bool skipPropertiesFile = false)
    {
        this._properties = properties;
        this._currentDir = currentDir;
        this._outputDir = outputDir;
        this.skipPropertiesFile = skipPropertiesFile;
        Run();
    }

    private void Run()
    {
        string[] files = Directory.GetFiles(this._currentDir);
        foreach (string file in files)
        {
            TreatFile(Path.GetFileName(file));
        }

        string[] folders = Directory.GetDirectories(this._currentDir);
        foreach (string folder in folders)
        {
            TreatFolder(Path.GetFileName(folder));
        }
    }

    private string ParseStringWithProperties(string str)
    {
        while (true)
        {
            Match match = this._rx.Match(str);
            if (!match.Success) return str;
            string[] inputs = match.Groups["input"].Value.Split(";");
            string formats = string.Empty;
            string value = inputs[0];
            //Find in dictionary
            if (this._properties.ContainsKey(value))
            {
                value = this._properties[value];
            }
            else
                Log.WriteLine($"Property {value} not found in dictionary", ConsoleColor.Red);

            if (inputs.Length >= 2)
            {
                formats = inputs[1];
                string[] f = formats.Split("=");
                if (f.Length <= 1) throw new ArgumentException("format defined and not specified");
                formats = f[1];
            }

            var sb = new StringBuilder();
            sb.Append(str[..match.Index]);
            sb.Append(Formatter.MultipleFormat(value, formats));
            sb.Append(str[(match.Index + match.Length)..]);
            str = sb.ToString();
        }
    }

    private void TreatFolder(string folderName)
    {
        string newName = ParseStringWithProperties(folderName);
        string oldFolderPath = Path.Combine(this._currentDir, folderName);
        string newFolder = Path.Combine(this._outputDir, newName);
        Directory.CreateDirectory(newFolder);
        Log.WriteLine($"created folder {newFolder}", ConsoleColor.Cyan);
        new Scaffold(this._properties, oldFolderPath, newFolder);
    }

    private DuplicateResult DuplicatedFile(string fileName)
    {
        string? inp = Input.ReadString($"{fileName} exists, do you want to overwrite it? (Y/n)");
        if (inp?.ToLower() != "n") return DuplicateResult.Overwrite;
        inp = Input.ReadString("do you want to skip this file? saying no will cancel the scaffold (Y/n)");
        if (inp?.ToLower() != "n") return DuplicateResult.Skip;
        Log.WriteLine("Scaffold cancelled, files created will NOT be deleted");
        return DuplicateResult.Cancel;
    }

    /**
     * @param fileName the name of the file to be copied
     */
    private void TreatFile(string fileName)
    {
        if (this.skipPropertiesFile && fileName.Equals("scaffold.properties")) return;
        string outFile = Path.Combine(this._outputDir, ParseStringWithProperties(fileName));
        string inFile = Path.Combine(this._currentDir, fileName);
        if (File.Exists(outFile))
        {
            switch (DuplicatedFile(outFile))
            {
                case DuplicateResult.Overwrite:
                    File.Delete(fileName);
                    break;
                case DuplicateResult.Skip:
                    return;
                case DuplicateResult.Cancel:
                    throw new CancelledScaffoldException(fileName);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        string[] lines = File.ReadAllLines(inFile);
        using var fs = new FileStream(outFile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        using var writer = new StreamWriter(fs);
        foreach (string line in lines)
        {
            writer.WriteLine(ParseStringWithProperties(line));
        }
        writer.Close();
        fs.Close();
        Log.WriteLine($"created file {outFile}", ConsoleColor.Cyan);
    }
}