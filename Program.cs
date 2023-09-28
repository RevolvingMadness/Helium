using System.Text;
using Helium.lexer;

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

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File does not exist");
            }

            string input = File.ReadAllText(path, Encoding.UTF8);

            Lexer lexer = new(input);
            List<Token> tokens = lexer.Lex();

            Console.WriteLine(ToString(tokens));
        }

        private static string ToString(List<Token> tokens)
        {
            string str = "[";

            foreach (object obj in tokens)
            {
                str += obj.ToString() + ", ";
            }

            str = str[..^2];

            str += "]";

            return str;
        }
    }
}
