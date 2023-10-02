
using Helium.compiler;
using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    abstract class ExpressionNode : Node
    {
        public abstract void Emit(ILProcessor processor, ProgramNode program);

        public abstract VariableType ToVariableType(ProgramNode program);
    }
}