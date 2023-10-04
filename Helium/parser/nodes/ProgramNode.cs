using Helium.compiler;
using Helium.logger;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    class ProgramNode : Node
    {
        public readonly List<StatementNode> statements;
        public readonly List<AssemblyDefinition> assemblies;
        public readonly string moduleName;
        public readonly AssemblyDefinition assemblyDefinition;
        public readonly AssemblyNameDefinition assemblyName;
        public readonly ModuleDefinition module;
        public readonly VariableTable variables;
        public List<string> assemblyPaths = new();
        public string outputPath = "";
        public ExpressionNode? returnValue;

        public ProgramNode(string moduleName, List<string> assemblyPaths, string outputPath)
        {
            statements = new();
            assemblies = new();

            this.assemblyPaths = assemblyPaths;
            this.outputPath = outputPath;
            this.moduleName = moduleName;

            assemblyName = new(moduleName, new Version(1, 0, 0));

            assemblyDefinition = AssemblyDefinition.CreateAssembly(
                assemblyName,
                moduleName,
                ModuleKind.Console
            );

            module = assemblyDefinition.MainModule;
            variables = new(this);

            LoadAssemblies();
        }

        public AssemblyDefinition Gen()
        {
            TypeDefinition mainClass = new(
                "",
                "Program",
                TypeAttributes.Abstract | TypeAttributes.Sealed,
                variables.Get("object").typeReference
            );

            MethodDefinition mainMethod = new(
                "Main",
                MethodAttributes.Public | MethodAttributes.Static,
                variables.Get("int").typeReference
            );

            ILProcessor processor = mainMethod.Body.GetILProcessor();

            foreach (StatementNode statement in statements)
            {
                statement.Gen(processor, this);
            }

            if (returnValue == null)
            {
                processor.Emit(OpCodes.Ldc_I4, 0);
                processor.Emit(OpCodes.Ret);
            }

            mainClass.Methods.Add(mainMethod);

            module.Types.Add(mainClass);

            assemblyDefinition.EntryPoint = mainMethod;

            return assemblyDefinition;
        }

        private void LoadAssemblies()
        {
            foreach (string referencePath in assemblyPaths)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Logger.Info("Adding assembly {0}", referencePath);
                assemblies.Add(AssemblyDefinition.ReadAssembly(referencePath));
            }
        }


        public TypeReference GetClassReference(string className)
        {
            List<TypeDefinition> foundTypes = new();

            foreach (AssemblyDefinition assembly in assemblies)
            {
                Logger.Info("Looking for class {0} in reference {1}", className, assembly.FullName);
                foreach (ModuleDefinition module in assembly.Modules)
                {
                    foreach (TypeDefinition class_ in module.Types)
                    {
                        if (class_.FullName == className)
                        {
                            foundTypes.Add(class_);
                        }
                    }
                }
            }

            if (foundTypes.Count == 1)
            {
                TypeReference typeReference = module.ImportReference(foundTypes[0]);
                return typeReference;
            }
            else if (foundTypes.Count == 0)
            {
                Logger.Error("Cannot find class {0}", className);
                return null;
            }
            else
            {
                Logger.Error("Found class {0} in more than 1 referenced assemblies", className);
                return null;
            }
        }

        public MethodReference GetMethodReference(string className, string methodName, List<string> parameterTypeNames)
        {
            List<TypeDefinition> foundTypes = new();

            foreach (AssemblyDefinition assembly in assemblies)
            {
                Logger.Info("Looking for method {0} in assembly {1}", methodName, assembly.FullName);
                foreach (ModuleDefinition module in assembly.Modules)
                {
                    foreach (TypeDefinition type in module.Types)
                    {
                        if (type.FullName == className)
                        {
                            foundTypes.Add(type);
                        }
                    }
                }
            }

            if (foundTypes.Count == 1)
            {
                TypeDefinition foundType = foundTypes[0];
                IEnumerable<MethodDefinition> methods = foundType.Methods.Where(m => m.Name == methodName);

                foreach (MethodDefinition method in methods)
                {
                    if (method.Parameters.Count != parameterTypeNames.Count)
                    {
                        continue;
                    }

                    bool allParametersMatch = true;

                    for (int i = 0; i < parameterTypeNames.Count; i++)
                    {
                        if (method.Parameters[i].ParameterType.FullName != parameterTypeNames[i])
                        {
                            allParametersMatch = false;
                            break;
                        }
                    }

                    if (!allParametersMatch)
                    {
                        continue;
                    }

                    return module.ImportReference(method);
                }

                Logger.Error("Cannot find method {0}", methodName);
            }
            else if (foundTypes.Count == 0)
            {
                Logger.Error("Cannot find method {0}", methodName);
            }
            else
            {
                Logger.Error("Found method {0} in more than 1 module", methodName);
            }

            return null;
        }
    }
}