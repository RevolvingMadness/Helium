using Helium.parser.nodes;

namespace Helium.parser.nodes {
    class IdentifierExpressionNode : ExpressionNode
    {
        public readonly string identifier;
        
        public IdentifierExpressionNode(string identifier)
        {
            this.identifier = identifier;
        }
    }
}