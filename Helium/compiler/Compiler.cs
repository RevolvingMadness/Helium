using System.Data;
using Helium.parser;
using Helium.parser.nodes;
using LLVMSharp;
using LLVMSharp.Interop;

namespace Helium.compiler
{
    class Compiler
    {
        private readonly ProgramNode programNode;
        private readonly LLVMModuleRef module;
        private readonly LLVMContextRef context;
        private readonly LLVMBuilderRef builder;
        private readonly string moduleName;

        private readonly VariableTable variables = new();

        public Compiler(ProgramNode programNode)
        {
            this.programNode = programNode;

            moduleName = "main";
            module = LLVMModuleRef.CreateWithName(moduleName);
            context = LLVMContextRef.Create();
            builder = LLVMBuilderRef.Create(context);
        }

        public void Compile()
        {
            CompileProgramNode(programNode);

            Console.WriteLine("Emitted IR:");
            Console.WriteLine(module.PrintToString());

            module.WriteBitcodeToFile("build/" + moduleName + ".bc");
        }

        private void CompileProgramNode(ProgramNode programNode)
        {
            LLVMTypeRef mainFunctionType = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, Array.Empty<LLVMTypeRef>());

            LLVMValueRef mainFunction = module.AddFunction(moduleName, mainFunctionType);

            LLVMBasicBlockRef mainFunctionBody = mainFunction.AppendBasicBlock("");
            builder.PositionAtEnd(mainFunctionBody);

            foreach (StatementNode statement in programNode.statements)
            {
                CompileStatementNode(statement);
            }

            builder.BuildRet(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0));
        }

        private void CompileStatementNode(StatementNode statement)
        {
            if (statement is AssignmentStatementNode assignmentStatement)
            {
                string name = assignmentStatement.name;
                VariableType? type = assignmentStatement.type;
                ExpressionNode? expression = assignmentStatement.expression;

                if (type == null || expression == null)
                {
                    throw new Exception("type or expression is null");
                }

                LLVMValueRef variable = builder.BuildAlloca(LLVMTypeRef.Int32, name);

                LLVMValueRef value = CompileExpression(expression);

                builder.BuildStore(value, variable);

                variables.Assign(LLVMTypeRef.Int32, name, value);
            }
            else
            {
                throw new Exception("Unknown statement '" + statement.GetType().Name + "'");
            }
        }

        private LLVMValueRef CompileExpression(ExpressionNode expression)
        {
            if (expression is IntegerExpressionNode integerExpression)
            {
                return LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, (ulong)integerExpression.value);
            }
            else if (expression is BinaryExpressionNode binaryExpression)
            {
                LLVMValueRef left = CompileExpression(binaryExpression.left);

                LLVMValueRef right = CompileExpression(binaryExpression.right);

                return binaryExpression.op switch
                {
                    lexer.TokenType.PLUS => builder.BuildAdd(left, right),
                    lexer.TokenType.HYPHEN => builder.BuildSub(left, right),
                    lexer.TokenType.STAR => builder.BuildMul(left, right),
                    lexer.TokenType.FSLASH => builder.BuildUDiv(left, right),
                    lexer.TokenType.PERCENT => builder.BuildFRem(left, right),
                    _ => throw new Exception("Unsupported binary operator '" + binaryExpression.op + "'")
                };
            }

            throw new Exception("Unknown expression type '" + expression.GetType().Name + "'");
        }
    }
}