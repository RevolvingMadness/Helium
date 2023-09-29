using Helium.parser;

namespace Helium.parser.helpers {
    class TypeHelper
    {
        public static Type fromString(string stringType) {
            Type type;

            switch (stringType)
            {
                case "int":
                    type = Type.INTEGER;
                    break;
                default:
                    throw new Exception("Unknown type '" + stringType + "'");
            }

            return type;
        }
    }
}