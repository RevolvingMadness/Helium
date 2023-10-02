using Helium.compiler;
using Helium.parser.nodes;

namespace Helium.checker
{
    class Checker
    {
        private readonly Dictionary<string, VariableType> variables;
        private readonly ProgramNode program;
        private readonly List<string> errors;

        public Checker(ProgramNode program)
        {
            this.program = program;

            variables = new();
            errors = new();
        }

        public bool HasNoErrors()
        {
            return CheckProgram(program);
        }

        public void PrintErrors()
        {
            foreach (string error in errors)
            {
                Console.WriteLine(error);
            }
        }

        private bool CheckProgram(ProgramNode program)
        {
            List<bool> statementResults = new();

            foreach (StatementNode statement in program.statements)
            {
                statementResults.Add(CheckStatement(statement));
            }

            return statementResults.All(result => result);
        }

        private bool CheckStatement(StatementNode statement)
        {
            if (statement is AssignmentStatementNode assignmentStatement)
            {
                string name = assignmentStatement.name;
                bool reassigning = assignmentStatement.reassigning;
                ExpressionNode expression = assignmentStatement.expression;

                if (reassigning)
                {
                    if (expression == null)
                    {
                        return Error("Cannot reassign variable '" + name + "' without value");
                    }

                    if (!variables.ContainsKey(name))
                    {
                        return Error("'" + name + "' is not defined");
                    }

                    return true;
                }

                if (variables.ContainsKey(name))
                {
                    return Error("'" + name + "' is already defined");
                }

                if (!CheckExpression(expression))
                {
                    return false;
                }

                variables.Add(name, expression.ToVariableType(program));

                return true;
            }
            else if (statement is ReturnStatementNode returnStatement)
            {
                if (!CheckExpression(returnStatement.expression))
                {
                    return false;
                }

                return true;
            }

            throw new Exception("Cannot check node '" + statement + "'");
        }

        private bool CheckExpression(ExpressionNode expression)
        {
            if (expression is IdentifierExpressionNode identifierExpression)
            {
                if (!variables.ContainsKey(identifierExpression.value))
                {
                    return Error("'" + identifierExpression.value + "' is not defined");
                }
            }
            else if (expression is BinaryExpressionNode binaryExpressionNode)
            {
                bool leftValid = CheckExpression(binaryExpressionNode.left);
                bool rightValid = CheckExpression(binaryExpressionNode.right);

                if (!(leftValid && rightValid))
                {
                    return false;
                }

                VariableType leftType = binaryExpressionNode.left.ToVariableType(program);
                VariableType rightType = binaryExpressionNode.right.ToVariableType(program);

                if (leftType == VariableType.NULL || rightType == VariableType.NULL)
                {
                    return Error("Cannot apply operator '" + binaryExpressionNode.op + "' to types '" + leftType + "' and '" + rightType + "'");
                }

                if (leftType == VariableType.BOOLEAN || rightType == VariableType.BOOLEAN)
                {
                    return Error("Cannot apply operator '" + binaryExpressionNode.op + "' to types '" + leftType + "' and '" + rightType + "'");
                }
            }

            return true;
        }

        private bool Error(string message)
        {
            errors.Add(message);
            return false;
        }
    }
}