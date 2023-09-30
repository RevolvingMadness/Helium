using Helium.parser.nodes;

namespace Helium.parser.nodes
{
    class IntegerExpressionNode : ExpressionNode
    {
        public readonly int value;

        public IntegerExpressionNode(int integer)
        {
            this.value = integer;
        }
    }
}