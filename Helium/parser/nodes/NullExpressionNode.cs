
using Helium.compiler;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    class NullExpressionNode : ExpressionNode
    {
        public NullExpressionNode()
        {

        }

        public override void Emit(ILProcessor processor, ProgramNode program)
        {
            processor.Emit(OpCodes.Ldc_I4_0);
        }

        public override VariableType ToVariableType(ProgramNode program)
        {
            throw new NotImplementedException();
        }

    }
}