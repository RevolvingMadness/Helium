using Helium.parser.nodes;

namespace Helium.parser.nodes {
    class IntegerExpressionNode : ExpressionNode
    {
        public readonly int integer;
        
        public IntegerExpressionNode(int integer)
        {
            this.integer = integer;
        }
    }
}