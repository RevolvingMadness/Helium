using Helium.lexer;

namespace Helium.helpers
{
    class TokenTypeHelper
    {
        public static bool IsAdditiveOperator(TokenType type)
        {
            return type == TokenType.PLUS || type == TokenType.HYPHEN;
        }

        public static bool IsMultiplicativeOperator(TokenType type)
        {
            return type == TokenType.STAR || type == TokenType.FSLASH || type == TokenType.PERCENT;
        }
    }
}