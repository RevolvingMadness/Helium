using System.Runtime.InteropServices;

namespace Helium.lexer
{
    class Lexer
    {
        string input;
        int position;
        Dictionary<char, TokenType> charMap;

        public Lexer(string input)
        {
            this.input = input;
            position = 0;
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
                        continue;
                    }
                    throw new Exception("Unknown character '" + Current() + "'");
                }
            }

            return tokens;
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