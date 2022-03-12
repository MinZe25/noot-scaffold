using System.Runtime.InteropServices;

namespace noot_scaffold;

public static class Input
{
    private static string? Read([Optional] string message, ConsoleColor? color = null)
    {
        Log.Write(message, color);
        return Console.ReadLine();
    }

    public static int ReadInt([Optional] string message,ConsoleColor? color = null)
    {
        string? input = Read(message, color);
        if (string.IsNullOrEmpty(input)) return 0;
        if (int.TryParse(input, out int ind) && ind >= 0) return ind;
        while (!int.TryParse(input, out ind) || ind < 0)
        {
            Log.WriteLine("Invalid selection, please try again", ConsoleColor.Red);
            input = Read(message);
        }

        return ind;
    }
    public static string? ReadString([Optional] string message, ConsoleColor? color = null)
    {
        return Read(message, color);
    }
}