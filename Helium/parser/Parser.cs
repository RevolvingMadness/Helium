using Helium.compiler;
using Helium.helpers;
using Helium.lexer;
using Helium.logger;
using Helium.parser.nodes;

namespace Helium.parser
{
    class Parser
    {
        private readonly List<Token> tokens;
        private readonly string moduleName;
        private int position = 0;

        public Parser(List<Token> tokens, string moduleName)
        {
            this.tokens = tokens;
            this.moduleName = moduleName;
        }

        public ProgramNode Parse(List<string> assemblyPaths, string outputPath)
        {
            ProgramNode programNode = new(moduleName, assemblyPaths, outputPath);

            while (IsNotEOF())
            {
                programNode.statements.Add(ParseStatement());
            }

            return programNode;
        }

        private bool IsNotEOF()
        {
            return !Current(TokenType.EOF);
        }

        private bool Current(TokenType type)
        {
            return Current().type == type;
        }

        private Token Current()
        {
            return Peek(0);
        }

        private Token Next()
        {
            return Peek(1);
        }

        private Token Peek(int amount)
        {
            return tokens[position + amount];
        }

        private Token Consume()
        {
            Token token = Current();

            position++;

            return token;
        }

        private Token Consume(TokenType type)
        {
            Token token = Consume();

            if (token.type != type)
            {
                Logger.Error("Expected {0}, got {1}", type, token.type);
            }

            return token;
        }

        private StatementNode ParseStatement()
        {
            if (Current(TokenType.IDENTIFIER))
            {
                if (Next(TokenType.LEFT_PARENTHESIS))
                {
                    return ParseFunctionCallStatement();
                }

                return ParseVariableAssignment();
            }
            else if (Current(TokenType.RETURN))
            {
                Consume(TokenType.RETURN);

                ExpressionNode expression = ParseExpression();

                Consume(TokenType.SEMICOLON);

                return new ReturnStatementNode(expression);
            }

            Logger.Error("Expected Statement, got {0}", Current().type);

            return null;
        }

        private StatementNode ParseVariableAssignment()
        {
            VariableType? type = null;

            if (Next(TokenType.IDENTIFIER))
            {
                string typeString = (string)Consume(TokenType.IDENTIFIER).value;
                type = TypeHelper.FromString(typeString);
            }

            string name = (string)Consume(TokenType.IDENTIFIER).value;


            Consume(TokenType.EQUALS);

            ExpressionNode expression = ParseExpression();

            Consume(TokenType.SEMICOLON);

            return new AssignmentStatementNode(type, name, expression);
        }

        private StatementNode ParseFunctionCallStatement()
        {
            string name = (string)Consume(TokenType.IDENTIFIER).value;

            Consume(TokenType.LEFT_PARENTHESIS);

            List<ExpressionNode> arguments = ParseArguments();
            
            Consume(TokenType.RIGHT_PARENTHESIS);

            Consume(TokenType.SEMICOLON);

            return new FunctionCallStatementNode(name, arguments);
        }

        private List<ExpressionNode> ParseArguments() {
            List<ExpressionNode> arguments = new();

            while(true) {
                arguments.Add(ParseExpression());

                if (Current(TokenType.RIGHT_PARENTHESIS)) {
                    break;
                }

                Consume(TokenType.COMMA);
            }

            return arguments;
        }

        private ExpressionNode ParseExpression()
        {
            return ParseAdditiveExpression();
        }

        private ExpressionNode ParseAdditiveExpression()
        {
            ExpressionNode left = ParseMultiplicativeExpression();

            while (TokenTypeHelper.IsAdditiveOperator(Current().type))
            {
                TokenType op = Consume().type;

                ExpressionNode right = ParseMultiplicativeExpression();

                left = new BinaryExpressionNode(left, op, right);
            }

            return left;
        }

        private ExpressionNode ParseMultiplicativeExpression()
        {
            ExpressionNode left = ParsePrimaryExpression();

            while (TokenTypeHelper.IsMultiplicativeOperator(Current().type))
            {
                TokenType op = Consume().type;

                ExpressionNode right = ParsePrimaryExpression();

                left = new BinaryExpressionNode(left, op, right);
            }

            return left;
        }

        private ExpressionNode ParsePrimaryExpression()
        {
            if (Current(TokenType.IDENTIFIER))
            {
                return new IdentifierExpressionNode((string)Consume(TokenType.IDENTIFIER).value);
            } else if (Current(TokenType.STRING))
            {
                return new StringExpressionNode((string)Consume(TokenType.STRING).value);
            }
            else if (Current(TokenType.INTEGER))
            {
                return new IntegerExpressionNode((int)Consume(TokenType.INTEGER).value);
            }
            else if (Current(TokenType.FLOAT))
            {
                return new FloatExpressionNode((float)Consume(TokenType.FLOAT).value);
            }
            else if (Current(TokenType.TRUE))
            {
                Consume(TokenType.TRUE);
                return new BooleanExpressionNode(true);
            }
            else if (Current(TokenType.FALSE))
            {
                Consume(TokenType.FALSE);
                return new BooleanExpressionNode(false);
            }
            else if (Current(TokenType.QUOTATIONMARK))
            {
                Consume(TokenType.QUOTATIONMARK);

                string str = (string)Consume(TokenType.IDENTIFIER).value;

                Consume(TokenType.QUOTATIONMARK);

                return new StringExpressionNode(str);
            }

            Logger.Error("Expected Expression, got {0}", Current().type);

            return null;
        }

        private bool Next(TokenType type)
        {
            return Next().type == type;
        }
    }
}