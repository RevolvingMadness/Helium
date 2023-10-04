using Helium.compiler;
using Helium.logger;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    class AssignmentStatementNode : StatementNode
    {
        public readonly string? type;
        public readonly string name;
        public readonly ExpressionNode expression;

        public AssignmentStatementNode(string? type, string name, ExpressionNode expression)
        {
            this.type = type;
            this.name = name;
            this.expression = expression;
        }

        public override void Gen(ILProcessor processor, ProgramNode program)
        {
            program.variables.Assign(processor, type, name, expression);
        }
    }
}