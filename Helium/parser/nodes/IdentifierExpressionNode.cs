using Helium.compiler;
using Mono.Cecil;
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
            int indexOfVariable = program.variables.Get(value).index;
            processor.Emit(OpCodes.Ldloc, indexOfVariable);
        }

        public override string ToTypeString(ProgramNode program)
        {
            return program.variables.Get(value).type;
        }
    }
}