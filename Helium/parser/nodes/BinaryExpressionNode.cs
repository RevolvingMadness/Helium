using Helium.compiler;
using Helium.lexer;
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
            throw new NotImplementedException();
        }

        public override VariableType ToVariableType(ProgramNode program)
        {
            throw new NotImplementedException();
        }
    }
}