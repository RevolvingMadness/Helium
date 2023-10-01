using Helium.helpers;
using Helium.lexer;
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

        public ProgramNode Parse()
        {
            ProgramNode programNode = new(moduleName);

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
                throw new Exception("Expected '" + type + "', got '" + token.type + "'");
            }

            return token;
        }

        private StatementNode ParseStatement()
        {
            if (Current(TokenType.IDENTIFIER))
            {
                bool reassigning = true;

                if (Next(TokenType.IDENTIFIER))
                {
                    Consume(TokenType.IDENTIFIER);
                    reassigning = false;
                }

                string name = Consume(TokenType.IDENTIFIER).value as string ?? throw new Exception("Identifier value is null???");


                if (Current(TokenType.SEMICOLON))
                {
                    Consume(TokenType.SEMICOLON);

                    return new AssignmentStatementNode(reassigning, name, new NullExpressionNode());
                }

                Consume(TokenType.EQUALS);

                ExpressionNode expression = ParseExpression();

                Consume(TokenType.SEMICOLON);

                return new AssignmentStatementNode(reassigning, name, expression);
            }
            else if (Current(TokenType.RETURN))
            {
                Consume(TokenType.RETURN);

                ExpressionNode expression = ParseExpression();

                Consume(TokenType.SEMICOLON);

                return new ReturnStatementNode(expression);
            }

            throw new Exception("Expected Statement, got '" + Current().type + "'");
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

#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8605
        private ExpressionNode ParsePrimaryExpression()
        {
            if (Current(TokenType.IDENTIFIER))
            {
                return new IdentifierExpressionNode((string)Consume(TokenType.IDENTIFIER).value);
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
            else if (Current(TokenType.NULL))
            {
                Consume(TokenType.NULL);
                return new NullExpressionNode();
            }
            else if (Current(TokenType.QUOTATIONMARK))
            {
                Consume(TokenType.QUOTATIONMARK);

                string str = (string)Consume(TokenType.IDENTIFIER).value;

                Consume(TokenType.QUOTATIONMARK);

                return new StringExpressionNode(str);
            }

            throw new Exception("Expected Expression, got '" + Current().type + "'");
        }

        private bool Next(TokenType type)
        {
            return Next().type == type;
        }
    }
}