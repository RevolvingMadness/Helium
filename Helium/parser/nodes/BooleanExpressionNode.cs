
using Helium.compiler;
using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    class BooleanExpressionNode : ExpressionNode
    {
        public readonly bool value;

        public BooleanExpressionNode(bool value)
        {
            this.value = value;
        }

        public override void Emit(ILProcessor processor, ProgramNode program)
        {
            processor.Emit(value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
        }

        public override VariableType ToVariableType(ProgramNode program)
        {
            return VariableType.BOOLEAN;
        }
    }
}