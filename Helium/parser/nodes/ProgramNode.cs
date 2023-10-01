using Helium.compiler;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    class ProgramNode : Node
    {
        public readonly List<StatementNode> statements;
        public readonly string moduleName;

        public readonly AssemblyDefinition assemblyDefinition;
        public readonly AssemblyNameDefinition assemblyName;
        public readonly ModuleDefinition module;
        public readonly VariableTable variables;
        public readonly Dictionary<VariableType, string> builtinTypes;

        public ProgramNode(string moduleName)
        {
            statements = new();
            this.moduleName = moduleName;

            assemblyName = new(moduleName, new Version(1, 0, 0));

            AssemblyDefinition.ReadAssembly(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8.1\mscorlib.dll");

            assemblyDefinition = AssemblyDefinition.CreateAssembly(
                assemblyName,
                moduleName,
                ModuleKind.Console
            );

            module = assemblyDefinition.MainModule;

            variables = new(this);

            builtinTypes = new()
            {
                { VariableType.OBJECT, "System.Object" },
                { VariableType.BOOLEAN, "System.Boolean" },
                { VariableType.INTEGER, "System.Int32" },
                { VariableType.STRING, "System.String" },
                { VariableType.VOID, "System.Void" },
            };
        }

        public AssemblyDefinition Gen()
        {
            TypeDefinition mainClass = new(
                "",
                "Program",
                TypeAttributes.Abstract | TypeAttributes.Sealed,
                module.TypeSystem.Object
            );

            MethodDefinition mainMethod = new(
                "Main",
                MethodAttributes.Private | MethodAttributes.Static,
                module.ImportReference(typeof(void))
            );

            ILProcessor processor = mainMethod.Body.GetILProcessor();

            // Define the variable
            var variableDefinition = new VariableDefinition(module.TypeSystem.Int32);
            mainMethod.Body.Variables.Add(variableDefinition);
            processor.Emit(OpCodes.Ldc_I4, 100);
            processor.Emit(OpCodes.Stloc, variableDefinition);
            processor.Emit(OpCodes.Ldloc, variableDefinition);

            mainClass.Methods.Add(mainMethod);

            module.Types.Add(mainClass);

            assemblyDefinition.EntryPoint = mainMethod;

            return assemblyDefinition;
        }
    }
}