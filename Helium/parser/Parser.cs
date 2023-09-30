using Helium.lexer;
using Helium.parser.nodes;
using Helium.parser.helpers;
using Helium.helpers;

namespace Helium.parser
{
    class Parser
    {
        private readonly List<Token> tokens;
        private int position = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public ProgramNode Parse()
        {
            ProgramNode programNode = new();

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
                VariableType? type = null;

                if (Next(TokenType.IDENTIFIER))
                {
                    string typeString = Consume(TokenType.IDENTIFIER).value as string ?? throw new Exception("Identifier value is null???");
                    type = TypeHelper.FromString(typeString);
                }

                string name = Consume(TokenType.IDENTIFIER).value as string ?? throw new Exception("Identifier value is null???");


                if (Current(TokenType.SEMICOLON))
                {
                    Consume(TokenType.SEMICOLON);

                    return new AssignmentStatementNode(type, name, null);
                }

                Consume(TokenType.EQUALS);

                ExpressionNode expression = ParseExpression();

                Consume(TokenType.SEMICOLON);

                return new AssignmentStatementNode(type, name, expression);
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

            throw new Exception("Expected Expression, got '" + Current().type + "'");
        }

        private bool Next(TokenType type)
        {
            return Next().type == type;
        }
    }
}