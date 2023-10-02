using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    abstract class StatementNode : Node
    {
        public abstract void Gen(ILProcessor processor, ProgramNode program);
    }
}