using Helium.compiler;
using Helium.logger;
using Helium.parser.nodes;
using Mono.Cecil;

namespace Helium.checker
{
    class Checker
    {
        private readonly Dictionary<string, string> variableTypes;
        private readonly ProgramNode program;

        public Checker(ProgramNode program)
        {
            this.program = program;

            variableTypes = new();
        }

        public bool HasErrors()
        {
            return ProgramHasErrors(program);
        }

        private bool ProgramHasErrors(ProgramNode program)
        {
            List<bool> statementsHaveErrors = new();

            foreach (StatementNode statement in program.statements)
            {
                statementsHaveErrors.Add(StatementHasErrors(statement));
            }

            return statementsHaveErrors.Any(v => v);
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
            else if (statement is FunctionCallStatementNode functionCallStatement)
            {
                List<bool> argumentsHaveErrors = new();

                foreach (ExpressionNode expression in functionCallStatement.arguments)
                {
                    argumentsHaveErrors.Add(ExpressionHasErrors(expression));
                }

                return argumentsHaveErrors.Any(v => v);
            }

            Logger.Error("Cannot check node {0}", statement);

            return true;
        }

        private bool AssignmentStatementHasErrors(AssignmentStatementNode assignmentStatement)
        {
            string name = assignmentStatement.name;
            string? type = assignmentStatement.type;
            ExpressionNode expression = assignmentStatement.expression;

            if (type == null)
            {
                if (expression == null)
                {
                    return Error("Cannot reassign variable {0} without value", name);
                }

                if (!variableTypes.ContainsKey(name))
                {
                    return Error("{0} is not defined", name);
                }

                return false;
            }

            TypeReference typeReference = program.variables.Get(type).typeReference;

            string expressionType = expression.ToTypeString(program);

            TypeReference expressionTypeReference = program.variables.Get(type).typeReference;

            if (typeReference != expressionTypeReference)
            {
                return Error("Cannot assign variable {0} with type {1} and value type {2}", name, type, expressionType);
            }

            if (variableTypes.ContainsKey(name))
            {
                return Error("{0} is already defined", name);
            }

            if (ExpressionHasErrors(expression))
            {
                return true;
            }

            variableTypes.Add(name, expression.ToTypeString(program));

            return false;
        }


        private bool ExpressionHasErrors(ExpressionNode expression)
        {
            if (expression is IdentifierExpressionNode identifierExpression)
            {
                if (!variableTypes.ContainsKey(identifierExpression.value))
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

                string leftType = binaryExpressionNode.left.ToTypeString(program);
                string rightType = binaryExpressionNode.right.ToTypeString(program);

                if (leftType == "bool" || rightType == "bool")
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