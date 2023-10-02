using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    class AssignmentStatementNode : StatementNode
    {
        public readonly bool reassigning;
        public readonly string name;
        public readonly ExpressionNode expression;

        public AssignmentStatementNode(bool reassigning, string name, ExpressionNode expression)
        {
            this.reassigning = reassigning;
            this.name = name;
            this.expression = expression;
        }

        public override void Gen(ILProcessor processor, ProgramNode program)
        {

        }
    }
}