
using Helium.compiler;

namespace Helium.parser.nodes
{
    abstract class ExpressionNode : Node
    {
        public abstract object ToValueRef(ProgramNode program);

        public abstract VariableType ToTypeRef(ProgramNode program);
    }
}