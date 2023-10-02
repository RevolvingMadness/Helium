using Helium.compiler;
using Helium.logger;

namespace Helium.parser
{
    class TypeHelper
    {
        public static VariableType FromString(string stringType)
        {
            VariableType variableType;

            switch (stringType)
            {
                case "int":
                    variableType = VariableType.INTEGER;
                    break;
                case "float":
                    variableType = VariableType.FLOAT;
                    break;
                case "boolean":
                    variableType = VariableType.BOOLEAN;
                    break;
                case "string":
                    variableType = VariableType.STRING;
                    break;
                default:
                    Logger.Error("Unknown type {0}", stringType);

                    variableType = VariableType.VOID;

                    break;
            }

            return variableType;
        }
    }
}