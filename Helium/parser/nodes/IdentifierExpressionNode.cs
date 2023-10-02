using Helium.compiler;
using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    class IdentifierExpressionNode : ExpressionNode
    {
        public readonly string value;

        public IdentifierExpressionNode(string value)
        {
            this.value = value;
        }

        public override void Emit(ILProcessor processor, ProgramNode program)
        {
            program.variables.Get(value).value.Emit(processor, program);
        }

        public override VariableType ToVariableType(ProgramNode program)
        {
            return program.variables.Get(value).type;
        }
    }
}