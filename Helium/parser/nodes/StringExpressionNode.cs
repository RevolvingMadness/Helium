using Helium.compiler;
using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    class StringExpressionNode : ExpressionNode
    {
        public readonly string value;

        public StringExpressionNode(string value)
        {
            this.value = value;
        }

        public override void Emit(ILProcessor processor, ProgramNode program)
        {
            processor.Emit(OpCodes.Ldstr, value);
        }

        public override VariableType ToVariableType(ProgramNode program)
        {
            return VariableType.STRING;
        }
    }
}