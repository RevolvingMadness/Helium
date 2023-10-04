
using Helium.compiler;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    class FloatExpressionNode : ExpressionNode
    {
        public readonly float value;

        public FloatExpressionNode(float value)
        {
            this.value = value;
        }

        public override void Emit(ILProcessor processor, ProgramNode program)
        {
            processor.Emit(OpCodes.Ldc_R4, value);
        }

        public override string ToTypeString(ProgramNode program)
        {
            return "float";
        }
    }
}