
using Helium.parser.nodes;
using Mono.Cecil;

namespace Helium.compiler
{
    class Variable
    {
        public readonly VariableType type;
        public readonly string name;
        public ExpressionNode value;
        public readonly object variableRef;

        public Variable(VariableType type, string name, ExpressionNode value, object variableRef)
        {
            this.type = type;
            this.name = name;
            this.value = value;
            this.variableRef = variableRef;
        }
    }
}