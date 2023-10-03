using Mono.Cecil.Cil;

namespace Helium.parser.nodes {
    class FunctionCallStatementNode : StatementNode
    {
        public readonly string name;
        public readonly List<ExpressionNode> arguments;

        public FunctionCallStatementNode(string name, List<ExpressionNode> arguments)
        {
            this.name = name;
            this.arguments = arguments;
        }

        public override void Gen(ILProcessor processor, ProgramNode program)
        {
            if (name == "print") {
                foreach (ExpressionNode argument in arguments) {
                    argument.Emit(processor, program);
                }

                processor.Emit(OpCodes.Call, program.writeLineMethodReference);
                return;
            }

            throw new NotImplementedException();
        }
    }
}