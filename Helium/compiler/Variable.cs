namespace Helium.compiler
{
    class Variable
    {
        public readonly VariableType type;
        public readonly string name;
        public readonly int index;

        public Variable(VariableType type, string name, int index)
        {
            this.type = type;
            this.name = name;
            this.index = index;
        }
    }
}