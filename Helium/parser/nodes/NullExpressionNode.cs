
using Helium.compiler;

namespace Helium.parser.nodes
{
    class NullExpressionNode : ExpressionNode
    {
        public NullExpressionNode()
        {

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