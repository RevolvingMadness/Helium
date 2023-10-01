using Helium.parser.nodes;
using Mono.Cecil;

namespace Helium.compiler
{
    class Compiler
    {
        public static void Compile(ProgramNode program)
        {
            string moduleName = program.moduleName;

            AssemblyDefinition module = program.Gen();

            Console.WriteLine("Emitted IR:");

            module.Write(moduleName + ".exe");
        }
    }
}