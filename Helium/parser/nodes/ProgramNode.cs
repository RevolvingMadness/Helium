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
        public Dictionary<VariableType, TypeReference> builtinTypeReferences;
        public List<string> referencePaths = new();
        public string outputPath = "";
        public ExpressionNode? returnValue;

        public ProgramNode(string moduleName)
        {
            statements = new();
            assemblies = new();

            this.moduleName = moduleName;

            assemblyName = new(moduleName, new Version(1, 0, 0));

            AssemblyDefinition.ReadAssembly(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.1\mscorlib.dll");

            assemblyDefinition = AssemblyDefinition.CreateAssembly(
                assemblyName,
                moduleName,
                ModuleKind.Console
            );

            module = assemblyDefinition.MainModule;

            variables = new(this);

            builtinTypeReferences = new();
        }

        public AssemblyDefinition Gen()
        {
            foreach (string referencePath in referencePaths)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Logger.Info("Adding assembly {0}", referencePath);
                assemblies.Add(AssemblyDefinition.ReadAssembly(referencePath));
            }

            builtinTypeReferences = GetBuiltinTypeReferences();

            MethodReference writeLineMethodReference = GetMethodReference("System.Console", "WriteLine", new() { "System.String" });

            TypeDefinition mainClass = new(
                "",
                "Program",
                TypeAttributes.Abstract | TypeAttributes.Sealed,
                builtinTypeReferences[VariableType.OBJECT]
            );

            MethodDefinition mainMethod = new(
                "Main",
                MethodAttributes.Public | MethodAttributes.Static,
                builtinTypeReferences[VariableType.INTEGER]
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

        private Dictionary<VariableType, TypeReference> GetBuiltinTypeReferences()
        {
            Dictionary<VariableType, string> builtinTypeNameMap = new()
            {
                { VariableType.VOID, "System.Void" },
                { VariableType.INTEGER, "System.Int32" },
                { VariableType.STRING, "System.String" },
                { VariableType.BOOLEAN, "System.Boolean" },
                { VariableType.OBJECT, "System.Object" },
            };

            Dictionary<VariableType, TypeReference> builtinTypeReferences = new();

            foreach (KeyValuePair<VariableType, string> entry in builtinTypeNameMap)
            {
                builtinTypeReferences.Add(entry.Key, GetClassReference(entry.Value));
            }

            return builtinTypeReferences;
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