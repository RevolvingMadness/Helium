using Helium.compiler;

namespace Helium.parser.nodes
{
    class StringExpressionNode : ExpressionNode
    {
        public readonly string value;

        public StringExpressionNode(string value)
        {
            this.value = value;
        }

        public override object ToValueRef(ProgramNode program)
        {
            throw new NotImplementedException();
        }

        public override VariableType ToTypeRef(ProgramNode program)
        {
            return VariableType.STRING;
        }
    }
}