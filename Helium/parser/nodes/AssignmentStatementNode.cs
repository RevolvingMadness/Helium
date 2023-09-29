using Helium.parser.nodes;

namespace Helium.parser.nodes {
    class AssignmentStatementNode : StatementNode
    {
        public readonly Type? type;
        public readonly string name;
        public readonly ExpressionNode expression;

        public AssignmentStatementNode(Type? type, string name, ExpressionNode expression)
        {
            this.type = type;
            this.name = name;
            this.expression = expression;
        }
    }
}