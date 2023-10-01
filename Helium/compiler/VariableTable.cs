using Helium.parser.nodes;

namespace Helium.compiler
{
    class VariableTable
    {
        private readonly ProgramNode program;
        private readonly List<Variable> variables;

        public VariableTable(ProgramNode program)
        {
            this.program = program;

            variables = new();
        }

        public void Assign(VariableType? type, string name, ExpressionNode expression)
        {
            if (type == null)
            {
                Reassign(name, expression);

                return;
            }

            object value = expression.ToValueRef(program);

            // object variable = program.builder.BuildAlloca((VariableType)type, name);

            // program.builder.BuildStore(value, variable);

            // variables.Add(new((LLVMTypeRef)type, name, value, variable));
        }

        public Variable Get(string name)
        {
            foreach (Variable variable in variables)
            {
                if (variable.name == name)
                {
                    return variable;
                }
            }

            throw new Exception("This should be checked in Checker.cs");
        }

        public void Reassign(string name, ExpressionNode expression)
        {
            if (expression is NullExpressionNode)
            {
                throw new Exception("This should be checked in Checker.cs");
            }

            for (int i = 0; i < variables.Count; i++)
            {
                Variable variable = variables[i];

                if (variable.name == name)
                {
                    variable.value = expression.ToValueRef(program);
                    // program.builder.BuildStore(variable.value, variable.variableRef);
                }
            }
        }
    }
}