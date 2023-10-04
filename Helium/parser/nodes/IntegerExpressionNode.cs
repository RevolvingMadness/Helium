
using Helium.compiler;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    class IntegerExpressionNode : ExpressionNode
    {
        public readonly int value;

        public IntegerExpressionNode(int value)
        {
            this.value = value;
        }

        public override void Emit(ILProcessor processor, ProgramNode program)
        {
            processor.Emit(OpCodes.Ldc_I4, value);
        }

        public override string ToTypeString(ProgramNode program)
        {
            return "int";
        }
    }
}