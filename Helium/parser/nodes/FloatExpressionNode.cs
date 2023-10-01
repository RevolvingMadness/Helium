
using Helium.compiler;

namespace Helium.parser.nodes
{
    class FloatExpressionNode : ExpressionNode
    {
        public readonly float value;

        public FloatExpressionNode(float value)
        {
            this.value = value;
        }

        public override object ToValueRef(ProgramNode program)
        {
            return new object();
        }

        public override VariableType ToTypeRef(ProgramNode program)
        {
            return VariableType.FLOAT;
        }
    }
}