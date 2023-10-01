
namespace Helium.parser.nodes
{
    class ReturnStatementNode : StatementNode
    {
        public readonly ExpressionNode expression;

        public ReturnStatementNode(ExpressionNode expression)
        {
            this.expression = expression;
        }

        public override void Gen(ProgramNode program)
        {

        }
    }
}