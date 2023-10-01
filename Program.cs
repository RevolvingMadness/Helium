using System.Text;
using Helium.checker;
using Helium.compiler;
using Helium.lexer;
using Helium.parser;
using Helium.parser.nodes;
using Mono.Options;

namespace Helium
{
    class Program
    {
        public static void Main(string[] args)
        {
            List<string> references = new();
            bool needsHelp = false;

            OptionSet options = new()
            {
                "Usage: helium <file> [options]",
                { "r=", "The {path} of an assembly to reference", references.Add },
                { "?|h|help", v => needsHelp = true }
            };

            if (needsHelp)
            {
                options.WriteOptionDescriptions(Console.Out);

                return;
            }

            if (args.Length == 0)
            {
                throw new Exception("Please provide a file to run");
            }

            string path = args[0];
            path = path.Replace("\\", "/");
            string name = path.Split("/")[^1].Split(".")[0];

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File does not exist");
            }

            string input = File.ReadAllText(path, Encoding.UTF8);

            Lexer lexer = new(input);
            List<Token> tokens = lexer.Lex();

            Parser parser = new(tokens, "main");
            ProgramNode programNode = parser.Parse();

            Checker checker = new(programNode);

            if (!checker.HasNoErrors())
            {
                checker.PrintErrors();
            }
            else
            {
                Compiler.Compile(programNode);
            }
        }
    }
}
