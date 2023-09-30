using System.Text;
using Helium.compiler;
using Helium.lexer;
using Helium.parser;
using Helium.parser.nodes;

namespace Helium
{
    class Program
    {
        public static void Main(string[] args)
        {
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

            Parser parser = new(tokens);
            ProgramNode programNode = parser.Parse();

            Compiler compiler = new(programNode);
            compiler.Compile();
        }
    }
}
