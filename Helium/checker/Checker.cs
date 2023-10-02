using System.Reflection.Metadata;
using Helium.compiler;
using Helium.logger;
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

        public bool HasErrors()
        {
            return ProgramHasErrors(program);
        }

        public void PrintErrors()
        {
            foreach (string error in errors)
            {
                Console.WriteLine(error);
            }
        }

        private bool ProgramHasErrors(ProgramNode program)
        {
            List<bool> statementsHaveErrors = new();

            foreach (StatementNode statement in program.statements)
            {
                statementsHaveErrors.Add(StatementHasErrors(statement));
            }

            return statementsHaveErrors.Any(result => result);
        }

        private bool StatementHasErrors(StatementNode statement)
        {
            if (statement is AssignmentStatementNode assignmentStatement)
            {
                return AssignmentStatementHasErrors(assignmentStatement);
            }
            else if (statement is ReturnStatementNode returnStatement)
            {
                if (ExpressionHasErrors(returnStatement.expression))
                {
                    return true;
                }

                return false;
            }

            Logger.Error("Cannot check node {0}", statement);

            return true;
        }

        private bool AssignmentStatementHasErrors(AssignmentStatementNode assignmentStatement)
        {
            string name = assignmentStatement.name;
            VariableType? type = assignmentStatement.type;
            ExpressionNode expression = assignmentStatement.expression;

            if (type == null)
            {
                if (expression == null)
                {
                    return Error("Cannot reassign variable {0} without value", name);
                }

                if (!variables.ContainsKey(name))
                {
                    return Error("{0} is not defined", name);
                }

                return false;
            }

            VariableType expressionType = expression.ToVariableType(program);

            if (type != expressionType)
            {
                return Error("Cannot assign variable {0} with type {1} and value type {2}", name, type, expressionType);
            }

            if (variables.ContainsKey(name))
            {
                return Error("{0} is already defined", name);
            }

            if (ExpressionHasErrors(expression))
            {
                return true;
            }

            variables.Add(name, expression.ToVariableType(program));

            return false;
        }


        private bool ExpressionHasErrors(ExpressionNode expression)
        {
            if (expression is IdentifierExpressionNode identifierExpression)
            {
                if (!variables.ContainsKey(identifierExpression.value))
                {
                    return Error("{0} is not defined", identifierExpression.value);
                }
            }
            else if (expression is BinaryExpressionNode binaryExpressionNode)
            {
                bool leftExpressionHasErrors = ExpressionHasErrors(binaryExpressionNode.left);
                bool rightExpressionHasErrors = ExpressionHasErrors(binaryExpressionNode.right);

                if (leftExpressionHasErrors || rightExpressionHasErrors)
                {
                    return true;
                }

                VariableType leftType = binaryExpressionNode.left.ToVariableType(program);
                VariableType rightType = binaryExpressionNode.right.ToVariableType(program);

                if (leftType == VariableType.BOOLEAN || rightType == VariableType.BOOLEAN)
                {
                    return Error("Cannot apply operator {0} to types {1} and {2}", binaryExpressionNode.op, leftType, rightType);
                }
            }

            return false;
        }

        private static bool Error(string message, params object[] args)
        {
            Logger.ErrorWithoutExit(message, args);
            return true;
        }
    }
}