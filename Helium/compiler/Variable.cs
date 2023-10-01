
namespace Helium.compiler
{
    class Variable
    {
        public readonly VariableType type;
        public readonly string name;
        public object value;
        public readonly object variableRef;

        public Variable(VariableType type, string name, object value, object variableRef)
        {
            this.type = type;
            this.name = name;
            this.value = value;
            this.variableRef = variableRef;
        }
    }
}