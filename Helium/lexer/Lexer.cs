namespace Helium.lexer
{
    class Lexer
    {
        readonly string input;
        int position = 0;
        Dictionary<char, TokenType> charMap;
        int column = 0;

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

                { '"', TokenType.QUOTATIONMARK },

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
                        throw new Exception("Unknown character '" + Current() + "'");
                    }

                }
            }

            tokens.Add(new Token(TokenType.EOF));

            return tokens;
        }

        private Token LexDigit()
        {
            string digit = "";
            bool isFloat = false;

            while (IsNotEOF() && (char.IsDigit(Current()) || Current() == '.'))
            {
                if (Current() == '.')
                {
                    if (isFloat)
                    {
                        break;
                    }

                    isFloat = true;
                }

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

            return identifier switch
            {
                "return" => new Token(TokenType.RETURN),
                "null" => new Token(TokenType.NULL),
                "true" => new Token(TokenType.TRUE),
                "false" => new Token(TokenType.FALSE),
                _ => new Token(TokenType.IDENTIFIER, identifier)
            };
        }

        private char Consume()
        {
            char character = Current();

            position++;
            column++;

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
            return position < input.Length;
        }
    }
}