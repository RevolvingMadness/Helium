namespace Helium.lexer
{
    class Token
    {
        public readonly TokenType type;
        public readonly object value;

        public Token(TokenType type, object value)
        {
            this.type = type;
            this.value = value;
        }

        public override string ToString()
        {
            return "Token(type=" + type + ", value=" + value + ")";
        }
    }
}