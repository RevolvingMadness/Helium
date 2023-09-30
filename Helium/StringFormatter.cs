using System.Collections;
using System.Reflection;

namespace Helium.parser.nodes
{
    class StringFormatter
    {
        public static string ToString(object obj)
        {
            if (obj is IList)
            {
                Console.WriteLine("a");
                return NodeListToString((List<Node>)obj);
            }

            if (obj is not Node)
            {
                string? str = obj.ToString() ?? throw new Exception("str is null???");
                return str;
            }

            return NodeToString((Node)obj);
        }

        private static string NodeToString(Node node)
        {
            string name = node.GetType().Name;

            string result = name + "(\n";

            string body = "";

            foreach (FieldInfo field in node.GetType().GetFields())
            {
                object? value = field.GetValue(node);

                body += field.Name + "=" + value + ",\n";
            }

            result += body + ")";

            return result;
        }

        private static string NodeListToString(List<Node> list)
        {
            string result = "[";

            foreach (Node node in list)
            {
                result += ToString(node);
                result += ", ";
            }

            if (list.Count > 0)
            {
                result = result[..^2];
            }

            result += "]";

            return result;
        }
    }
}