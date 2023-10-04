using Helium.helpers;
using Helium.lexer;
using Helium.logger;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    class BinaryExpressionNode : ExpressionNode
    {
        public readonly ExpressionNode left;
        public readonly lexer.TokenType op;
        public readonly ExpressionNode right;

        public BinaryExpressionNode(ExpressionNode left, lexer.TokenType op, ExpressionNode right)
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

        public override string ToTypeString(ProgramNode program)
        {
            string leftType = left.ToTypeString(program);
            string rightType = right.ToTypeString(program);

            List<string> typePrecedence = new() {
                "bool",
                "void",
                "string",
                "float",
                "int"
            };

            foreach (string type in typePrecedence)
            {
                if (leftType == type || rightType == type)
                {
                    return type;
                }
            }

            Logger.Error("Unsupported types {0} and/or {1}", leftType, rightType);

            return "";
        }
    }
}