
using Helium.compiler;

namespace Helium.parser.nodes
{
    class IntegerExpressionNode : ExpressionNode
    {
        public readonly int value;

        public IntegerExpressionNode(int value)
        {
            this.value = value;
        }

        public override object ToValueRef(ProgramNode program)
        {
            return new object();
        }

        public override VariableType ToTypeRef(ProgramNode program)
        {
            return VariableType.INTEGER;
        }
    }
}