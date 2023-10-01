
using Helium.compiler;

namespace Helium.parser.nodes
{
    class BooleanExpressionNode : ExpressionNode
    {
        public readonly bool value;

        public BooleanExpressionNode(bool value)
        {
            this.value = value;
        }

        public override object ToValueRef(ProgramNode program)
        {
            return new object();
        }

        public override VariableType ToTypeRef(ProgramNode program)
        {
            return VariableType.BOOLEAN;
        }
    }
}