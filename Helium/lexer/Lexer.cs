namespace Helium.lexer
{
    class Lexer
    {
        readonly string input;
        int position = 0;
        Dictionary<char, TokenType> charMap;
        int column = 0;
        readonly int errorWidth = 5;

        public Lexer(string input)
        {
            this.input = input;
            charMap = new()
            {
                { '=', TokenType.EQUALS },
                { '+', TokenType.PLUS },
                { '-', TokenType.HYPHEN },
                { '*', TokenType.STAR },
                { '/', TokenType.FSLASH },
                { '%', TokenType.PERCENT },

                { ';', TokenType.SEMICOLON },
            };
        }

        public List<Token> Lex()
        {
            List<Token> tokens = new();

            while (IsNotEOF())
            {
                if (char.IsWhiteSpace(Current()))
                {
                    Consume();
                }
                else if (char.IsLetter(Current()) || Current() == '_')
                {
                    tokens.Add(LexIdentifier());
                }
                else if (char.IsDigit(Current()) || (Current() == '.' && char.IsDigit(Next())))
                {
                    tokens.Add(LexDigit());
                }
                else
                {
                    if (charMap.ContainsKey(Current()))
                    {
                        tokens.Add(new Token(charMap.GetValueOrDefault(Consume())));
                    }
                    else
                    {
                        ThrowError("Unknown character '" + Current() + "'");
                    }

                }

                column += 1;
            }

            return tokens;
        }

        private void ThrowError(string message)
        {
            string bounds = "";

            int leftBound;
            int rightBound;

            for (leftBound = 0; leftBound < errorWidth; leftBound++)
            {
                if (column - leftBound - 1 < 0)
                {
                    break;
                }

                bounds += input[column - leftBound - 1];
            }

            bounds += input[column];

            for (rightBound = 0; rightBound < errorWidth; rightBound++)
            {
                if (column + rightBound + 1 > input.Length - 1)
                {
                    break;
                }

                bounds += input[column + rightBound + 1];
            }

            Console.Error.WriteLine(message + ":");
            Console.Error.WriteLine(bounds);

            Console.Error.Write(new string('^', leftBound));

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.Write('^');
            Console.ForegroundColor = ConsoleColor.White;

            Console.Error.Write(new string('^', rightBound));

            Environment.Exit(1);
        }

        private Token LexDigit()
        {
            string digit = "";
            bool isFloat = false;

            while (IsNotEOF() && (char.IsDigit(Current()) || Current() == '.'))
            {
                digit += Consume();
            }

            if (isFloat)
            {
                return new Token(TokenType.FLOAT, float.Parse(digit));
            }

            return new Token(TokenType.INTEGER, int.Parse(digit));
        }

        private char Next()
        {
            return Peek(1);
        }

        private Token LexIdentifier()
        {
            string identifier = "";

            while (IsNotEOF() && (char.IsLetterOrDigit(Current()) || Current() == '_'))
            {
                identifier += Consume();
            }

            return new Token(TokenType.IDENTIFIER, identifier);
        }

        private char Consume()
        {
            char character = Current();

            position++;

            return character;
        }

        private char Current()
        {
            return Peek(0);
        }

        private char Peek(int amount)
        {
            return input[position + amount];
        }

        private bool IsNotEOF()
        {
            return position <= input.Length - 1;
        }
    }
}