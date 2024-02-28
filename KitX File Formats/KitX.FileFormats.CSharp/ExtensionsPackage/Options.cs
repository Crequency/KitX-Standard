using CommandLine;

namespace KitX.FileFormats.CSharp.ExtensionsPackage;

public class Options
{
    [Option('v', "verbose", HelpText = "Prints all messages to standard output.")]
    public bool Verbose { get; set; } = false;
}
