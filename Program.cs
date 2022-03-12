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
        var str = "__hello.world how_are;format=camel__";
        new Scaffold(null, null, null).ParseStringWithProperties(str);

        str = "__hello.world how_are with packaged;format=packaged__";
        new Scaffold(null, null, null).ParseStringWithProperties(str);
        str = "__hello.world how_are with packaged uppercase;format=packaged,upper__";
        new Scaffold(null, null, null).ParseStringWithProperties(str);
    }
}