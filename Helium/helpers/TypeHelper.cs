using Helium.compiler;

namespace Helium.parser
{
    class TypeHelper
    {
        public static VariableType FromString(string stringType)
        {
            return stringType switch
            {
                "int" => VariableType.INTEGER,
                "float" => VariableType.FLOAT,
                "boolean" => VariableType.BOOLEAN,
                "string" => VariableType.STRING,
                _ => throw new Exception("Unknown type '" + stringType + "'"),
            };
        }
    }
}