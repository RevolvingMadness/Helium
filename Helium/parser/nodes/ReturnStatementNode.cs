using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
    class ReturnStatementNode : StatementNode
    {
        public readonly ExpressionNode expression;

        public ReturnStatementNode(ExpressionNode expression)
        {
            this.expression = expression;
        }

        public override void Gen(ILProcessor processor, ProgramNode program)
        {
            program.returnValue = expression;
            expression.Emit(processor, program);
            processor.Emit(OpCodes.Ret);
        }
    }
}