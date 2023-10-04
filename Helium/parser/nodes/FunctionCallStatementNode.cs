using Helium.logger;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Helium.parser.nodes
{
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
            if (program.variables.Get(name).typeReference != program.variables.Get("function").typeReference)
            {
                Logger.Error("{0} is not a function", name);
            };

            foreach (ExpressionNode argument in arguments)
            {
                argument.Emit(processor, program);
            }

            processor.Emit(OpCodes.Ldstr, name);

            processor.Emit(OpCodes.Call);
        }
    }
}