using Helium.lexer;

namespace Helium.helpers
{
    class TokenTypeHelper
    {
        public static bool IsAdditiveOperator(TokenType op)
        {
            return op == TokenType.PLUS || op == TokenType.HYPHEN;
        }
        public static bool IsMultiplicativeOperator(TokenType op)
        {
            return op == TokenType.STAR || op == TokenType.FSLASH || op == TokenType.PERCENT;
        }

        public static bool IsBinaryOperator(TokenType op)
        {
            return op == TokenType.PLUS || op == TokenType.HYPHEN || op == TokenType.STAR || op == TokenType.FSLASH || op == TokenType.PERCENT;
        }
    }
}