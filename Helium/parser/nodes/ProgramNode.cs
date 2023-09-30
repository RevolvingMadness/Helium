using Helium;

namespace Helium.parser.nodes
{
    class ProgramNode : Node
    {
        public readonly List<StatementNode> statements = new();
    }
}