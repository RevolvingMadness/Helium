using Helium.parser.nodes;
using Mono.Cecil;

namespace Helium.compiler
{
    class Compiler
    {
        public static void Compile(ProgramNode program)
        {
            string moduleName = program.moduleName;

            string outputPath = program.outputPath;

            AssemblyDefinition module = program.Gen();

            if (outputPath == "")
            {
                module.Write(moduleName + ".exe");
            }
            else
            {
                module.Write(outputPath);
            }
        }
    }
}