using System.Text;
using Helium.checker;
using Helium.compiler;
using Helium.lexer;
using Helium.logger;
using Helium.parser;
using Helium.parser.nodes;
using Mono.Options;

namespace Helium
{
    class Program
    {
        public static void Main(string[] args)
        {
            List<string> assemblyPaths = new();
            bool needsHelp = false;
            string outputPath = "";
            string sourcePath = "";

            OptionSet options = new()
            {
                "Usage: helium <file> [options]",
                { "r=", "The {path} of an assembly to reference", assemblyPaths.Add },
                { "o=", "The output {path} of an assembly to reference", path => outputPath = path },
                { "h|help", v => needsHelp = true },
                { "<>", path => sourcePath = path }
            };

            options.Parse(args);

            if (needsHelp)
            {
                options.WriteOptionDescriptions(Console.Out);

                return;
            }

            if (sourcePath == "")
            {
                Logger.Error("Please provide a file to run");
            }

            if (!File.Exists(sourcePath))
            {
                Logger.Error("File {0} does not exist", sourcePath);
            }

            string input = File.ReadAllText(sourcePath, Encoding.UTF8);

            Lexer lexer = new(input);
            List<Token> tokens = lexer.Lex();

            Parser parser = new(tokens, "main");
            ProgramNode programNode = parser.Parse(assemblyPaths, outputPath);

            Checker checker = new(programNode);

            if (!checker.HasErrors())
            {
                Compiler.Compile(programNode);
            }
        }
    }
}
