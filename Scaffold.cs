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
    private Regex _rx = new("{{(?<input>.*?)}}", RegexOptions.Compiled);

    public Scaffold(Dictionary<string, string> properties, string currentDir, string outputDir)
    {
        this._properties = properties;
        this._currentDir = currentDir;
        this._outputDir = outputDir;
    }

    public string ParseStringWithProperties(string str)
    {
        Match match = this._rx.Match(str);
        if (!match.Success) return str;
        string[] inputs = match.Groups["input"].Value.Split(";");
        string formats = string.Empty;
        string value = inputs[0];
        if (inputs.Length >= 2)
        {
            formats = inputs[1];
            string[] f = formats.Split("=");
            if (f.Length <= 1) throw new ArgumentException("format defined and not specified");
            formats = f[1];
        }

        var sb = new StringBuilder();
        sb.Append(str.Substring(0, match.Index));
        sb.Append(ScaffoldFormatter.MultipleFormat(value, formats));
        sb.Append(str.Substring(match.Index + match.Length));
        return ParseStringWithProperties(sb.ToString());
    }

    public void TreatFolder(string folderName)
    {
        //Detects a folder
        //Reads it's name
        //Parse the name if necessary
        //Create the new folder on the ouputDir
        //Create a new Scaffold inside the folder
        string newName = ParseStringWithProperties(folderName);
        string oldFolderPath = Path.Combine(this._currentDir, folderName);
        string newFolder = Path.Combine(this._outputDir, newName);
        Directory.CreateDirectory(newFolder);
        var newScaffold = new Scaffold(this._properties, oldFolderPath, newFolder);
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
    public void TreatFile(string fileName)
    {
        //Reads the file
        //Parse the file if necessary, on a buffer
        //Write the buffer on the outputDir
        //Go to the next file
        var outFile = ParseStringWithProperties(Path.Combine(this._outputDir, fileName));
        var inFile = Path.Combine(this._currentDir, fileName);
        if (File.Exists(outFile))
        {
            switch (DuplicatedFile(fileName))
            {
                case DuplicateResult.Overwrite:
                    File.Delete(fileName);
                    break;
                case DuplicateResult.Skip:
                    return;
                case DuplicateResult.Cancel:
                    throw new CancelledScaffoldException(fileName);
                    return;
            }
        }

        string[] lines = File.ReadAllLines(inFile);
        using (var fs = new FileStream(outFile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            using (var writer = new StreamWriter(fs))
            {
                foreach (string line in lines)
                {
                    writer.WriteLine(ParseStringWithProperties(line));
                }
            }
        }
    }
}