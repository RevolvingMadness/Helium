using Helium.lexer;
using Helium.logger;
using Mono.Cecil.Cil;

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

        internal static OpCode ToOpCode(TokenType op)
        {
            OpCode? opCode = null;

            switch (op)
            {
                case TokenType.PLUS:
                    opCode = OpCodes.Add;
                    break;
                case TokenType.HYPHEN:
                    opCode = OpCodes.Sub;
                    break;
                case TokenType.STAR:
                    opCode = OpCodes.Mul;
                    break;
                case TokenType.FSLASH:
                    opCode = OpCodes.Div;
                    break;
                case TokenType.PERCENT:
                    opCode = OpCodes.Rem;
                    break;
            };

            if (opCode == null)
            {
                Logger.Error("Unsupported binary operator {0}", op);

                return OpCodes.Add;
            }

            return (OpCode)opCode;
        }
    }
}