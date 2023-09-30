using LLVMSharp.Interop;

namespace Helium.compiler
{
    class Variable
    {
        public readonly LLVMTypeRef type;
        public readonly string name;
        public readonly LLVMValueRef value;

        public Variable(LLVMTypeRef type, string name, LLVMValueRef value)
        {
            this.type = type;
            this.name = name;
            this.value = value;
        }
    }
}