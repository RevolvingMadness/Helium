using Helium.compiler;

namespace Helium.parser.nodes
{
    class IdentifierExpressionNode : ExpressionNode
    {
        public readonly string value;

        public IdentifierExpressionNode(string value)
        {
            this.value = value;
        }

        public override object ToValueRef(ProgramNode program)
        {
            return new object();
        }

        public override VariableType ToTypeRef(ProgramNode program)
        {
            return VariableType.NULL;
        }
    }
}