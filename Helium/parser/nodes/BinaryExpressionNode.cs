using Helium.compiler;
using Helium.helpers;
using Helium.lexer;
using Helium.logger;
using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    class BinaryExpressionNode : ExpressionNode
    {
        public readonly ExpressionNode left;
        public readonly TokenType op;
        public readonly ExpressionNode right;

        public BinaryExpressionNode(ExpressionNode left, TokenType op, ExpressionNode right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }

        public override void Emit(ILProcessor processor, ProgramNode program)
        {
            left.Emit(processor, program);
            right.Emit(processor, program);
            processor.Emit(TokenTypeHelper.ToOpCode(op));
        }

        public override VariableType ToVariableType(ProgramNode program)
        {
            VariableType leftType = left.ToVariableType(program);
            VariableType rightType = right.ToVariableType(program);

            List<VariableType> typePrecedence = new() {
                VariableType.BOOLEAN,
                VariableType.VOID,
                VariableType.STRING,
                VariableType.FLOAT,
                VariableType.INTEGER,
            };

            foreach (VariableType type in typePrecedence)
            {
                if (leftType == type || rightType == type)
                {
                    return type;
                }
            }

            Logger.Error("Unsupported types {0} and/or {1}", leftType, rightType);

            return VariableType.VOID;
        }
    }
}