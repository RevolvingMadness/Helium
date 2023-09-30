using LLVMSharp.Interop;

namespace Helium.compiler
{
    class VariableTable
    {
        private readonly List<Variable> variables;

        public VariableTable()
        {
            variables = new();
        }

        public void Assign(LLVMTypeRef type, string name, LLVMValueRef value)
        {
            variables.Add(new(type, name, value));
        }

        public LLVMValueRef GetOrThrow(string name)
        {
            foreach (Variable variable in variables)
            {
                if (variable.name == name)
                {
                    return variable.value;
                }
            }

            throw new Exception("'" + name + "' is not defined");
        }
    }
}