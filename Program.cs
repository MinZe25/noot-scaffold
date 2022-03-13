// See https://aka.ms/new-console-template for more information

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using noot_scaffold.Exceptions;

namespace noot_scaffold;

public static class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var main = new Main(args);
        }
        catch (CancelledScaffoldException e)
        {
            Log.WriteLine($"Scaffolding cancelled in file {e.Message}. All modified files will not be reverted.",
                ConsoleColor.Red);
        }
    }
}