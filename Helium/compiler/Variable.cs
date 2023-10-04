using Mono.Cecil;

namespace Helium.compiler
{
    class Variable
    {
        public readonly TypeReference typeReference;
        public readonly string type;
        public readonly string name;
        public readonly int index;

        public Variable(TypeReference typeReference, string type, string name, int index)
        {
            this.typeReference = typeReference;
            this.name = name;
            this.type = type;
            this.index = index;
        }
    }
}