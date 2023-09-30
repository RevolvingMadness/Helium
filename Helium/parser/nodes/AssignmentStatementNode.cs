using Helium.parser;

namespace Helium.parser.nodes
{
    class AssignmentStatementNode : StatementNode
    {
        public readonly VariableType? type;
        public readonly string name;
        public readonly ExpressionNode? expression;

        public AssignmentStatementNode(VariableType? type, string name, ExpressionNode? expression)
        {
            this.type = type;
            this.name = name;
            this.expression = expression;
        }
    }
}