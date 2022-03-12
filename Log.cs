using System.Runtime.InteropServices;

namespace noot_scaffold;

public static class Log
{
    private static ConsoleColor _defaultTextColor = ConsoleColor.White;

    private static void InternalWrite(string? message, [Optional] ConsoleColor? color, bool newLine)
    {
        color ??= _defaultTextColor;
        Console.ForegroundColor = color.Value;
        if (newLine)
            Console.WriteLine(message);
        else
            Console.Write(message);
        Console.ResetColor();
    }

    public static void WriteLine(string? message, [Optional] ConsoleColor? color)
    {
        InternalWrite(message, color, true);
    }

    public static void Write(string? message, [Optional] ConsoleColor? color)
    {
        InternalWrite(message, color, false);
    }

    public static ConsoleColor SetDefaultTextColor(ConsoleColor color)
    {
        return _defaultTextColor;
    }
}