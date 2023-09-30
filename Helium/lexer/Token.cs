namespace Helium.lexer
{
    class Token
    {
        public readonly TokenType type;
        public readonly object? value;

        public Token(TokenType type, object value)
        {
            this.type = type;
            this.value = value;
        }

        public Token(TokenType type)
        {
            this.type = type;
        }

        public override string ToString()
        {
            if (value == null)
            {
                return "Token(type=" + type + ")";
            }

            return "Token(type=" + type + ", value=" + value + ")";
        }
    }
}