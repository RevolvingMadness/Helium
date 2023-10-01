
namespace Helium.parser.nodes
{
    abstract class StatementNode : Node
    {
        public abstract void Gen(ProgramNode program);
    }
}