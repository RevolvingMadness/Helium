namespace Helium.parser.nodes
{
    class IdentifierExpressionNode : ExpressionNode
    {
        public readonly string value;

        public IdentifierExpressionNode(string identifier)
        {
            this.value = identifier;
        }
    }
}