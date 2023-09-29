using Helium.parser.nodes;

namespace Helium.parser.nodes {
    class NodePrinter
    {
        public static string NodeToString(Node node) {
            if (node is ProgramNode programNode) {
                return "ProgramNode(statements=" + NodeListToString(programNode.statements.Cast<Node>().ToList()) + ")";
            } else if (node is AssignmentStatementNode assignmentStatementNode) {
                if (assignmentStatementNode.type == null) {
                    return "AssignmentStatementNode(name=" + assignmentStatementNode.name + ", expression=" + NodeToString(assignmentStatementNode.expression) + ")";
                }
                
                return "AssignmentStatementNode(name=" + assignmentStatementNode.name + ", type=" + assignmentStatementNode.type + ", expression=" + NodeToString(assignmentStatementNode.expression) + ")";
            } else if (node is IntegerExpressionNode integerExpressionNode) {
                return "IntegerExpressionNode(integer=" + integerExpressionNode.integer + ")";
            } else if (node is BinaryExpressionNode binaryExpressionNode) {
                return "BinaryExpressionNode(left=" + NodeToString(binaryExpressionNode.left) + ", op=" + binaryExpressionNode.op + ", right=" + NodeToString(binaryExpressionNode.right) + ")";
            }

            throw new Exception("Cannot convert node '" + node.GetType().Name + "' to string");
        }

        private static string NodeListToString(List<Node> list) {
            string result = "[";

            foreach (Node node in list) {
                result += NodeToString(node);
                result += ", ";
            }

            if (list.Count > 0) {
                result = result[..^2];
            }

            result += "]";

            return result;
        } 
    }
}