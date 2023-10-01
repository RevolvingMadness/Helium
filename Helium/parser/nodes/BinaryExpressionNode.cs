using Helium.compiler;
using Helium.lexer;

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

        public override object ToValueRef(ProgramNode program)
        {
            return new object();
        }

        public override VariableType ToTypeRef(ProgramNode program)
        {
            return left.ToTypeRef(program);
        }
    }
}