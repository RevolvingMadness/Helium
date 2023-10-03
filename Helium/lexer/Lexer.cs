#pragma warning disable CS8625
using Helium.logger;

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
                { ',', TokenType.COMMA },

                { '(', TokenType.LEFT_PARENTHESIS },
                { ')', TokenType.RIGHT_PARENTHESIS },
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
                else if (Current() == '"')
                {
                    tokens.Add(LexString());
                }
                else
                {
                    if (charMap.ContainsKey(Current()))
                    {
                        char chr = Consume();

                        tokens.Add(new Token(charMap.GetValueOrDefault(chr), null));
                    }
                    else
                    {
                        Logger.Error("Unknown character {0}", Current());
                    }

                }
            }

            tokens.Add(new Token(TokenType.EOF, null));

            return tokens;
        }

        private Token LexString()
        {
            string str = "";

            Consume();

            while (Current() != '"') {
                str += Consume();
            }

            Consume();

            return new(TokenType.STRING, str);
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
                "return" => new Token(TokenType.RETURN, null),
                "true" => new Token(TokenType.TRUE, true),
                "false" => new Token(TokenType.FALSE, false),
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