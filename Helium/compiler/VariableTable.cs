using Helium.logger;
using Helium.parser.nodes;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Helium.compiler
{
    class VariableTable
    {
        private readonly ProgramNode program;
        private readonly Dictionary<string, Variable> variables;
        private int variableIndex;

        public VariableTable(ProgramNode program)
        {
            this.program = program;

            variables = new();

            variableIndex = 0;
        }

        public void Assign(ILProcessor processor, VariableType? type, string name, ExpressionNode expression)
        {
            if (type == null)
            {
                Reassign(processor, name, expression);

                return;
            }

            TypeReference typeReference = program.builtinTypeReferences[(VariableType)type];

            VariableDefinition variableDefinition = new(typeReference);

            processor.Body.Variables.Add(variableDefinition);

            expression.Emit(processor, program);
            processor.Emit(OpCodes.Stloc, variableIndex);

            Variable variable = new((VariableType)type, name, variableIndex);

            variables.Add(name, variable);

            variableIndex++;
        }

        private void Reassign(ILProcessor processor, string name, ExpressionNode expression)
        {
            expression.Emit(processor, program);
            processor.Emit(OpCodes.Stloc, GetVariableIndex(name));
        }

        private int GetVariableIndex(string name)
        {
            return Get(name).index;
        }

        public Variable Get(string name)
        {
            Variable? variable = variables.GetValueOrDefault(name);

            if (variable == null)
            {
                Logger.Error("{0} is not defined (This should be checked in Checker.cs)", name);
            }

            return variables.GetValueOrDefault(name);
        }
    }
}