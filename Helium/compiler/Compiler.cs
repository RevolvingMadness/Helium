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
                if (OperatingSystem.IsWindows()) {
                    module.Write(moduleName + ".exe");
                } else {
                    module.Write(moduleName);
                }
            }
            else
            {
                module.Write(outputPath);
            }
        }
    }
}