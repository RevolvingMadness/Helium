using Helium.parser;

namespace Helium.parser.helpers
{
    class TypeHelper
    {
        public static VariableType FromString(string stringType)
        {
            return stringType switch
            {
                "int" => VariableType.INTEGER,
                _ => throw new Exception("Unknown type '" + stringType + "'"),
            };
        }
    }
}