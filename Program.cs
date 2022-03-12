// See https://aka.ms/new-console-template for more information

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace noot_scaffold;

public class Program
{
    static void Main(string[] args)
    {
        // var main = new Main(args);
        // Regex _rx = new("__(?<input>.*)__", RegexOptions.Compiled);
        // var str = "{{hello.world how_are;format=camel}}";
        // new Scaffold(null, null, null).ParseStringWithProperties(str);
        // str = "{{hello.world how_are with packaged;format=packaged}}";
        // new Scaffold(null, null, null).ParseStringWithProperties(str);
        // str = "{{hello.world how_are with packaged uppercase;format=packaged,upper}}";
        // new Scaffold(null, null, null).ParseStringWithProperties(str);
        var str = "__{{hello world;format=packaged,upper}}__{{man wow;format=camel}}__";
        Log.WriteLine(new Scaffold(null, null, null).ParseStringWithProperties(str)
            , ConsoleColor.Magenta);
    }
}