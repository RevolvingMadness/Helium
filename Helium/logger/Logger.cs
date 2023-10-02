using System.Text;

namespace Helium.logger
{
    class Logger
    {
        private static void Log(string type, ConsoleColor color, string message, params object[] args)
        {
            string evaluatedMessage = message;

            for (int i = 0; i < args.Length; i++)
            {
                evaluatedMessage = evaluatedMessage.Replace("{" + i + "}", "'" + args[i].ToString() + "'");
            }

            Console.ForegroundColor = color;
            Console.Write($"[{type.ToUpper()}] ");
            Console.ResetColor();

            Console.WriteLine(evaluatedMessage);
        }

        public static void Info(string message, params object[] args)
        {
            Log("info", ConsoleColor.DarkGray, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            ErrorWithoutExit(message, args);

            Environment.Exit(1);
        }

        internal static void ErrorWithoutExit(string message, params object[] args)
        {
            Log("error", ConsoleColor.DarkRed, message, args);
        }
    }
}