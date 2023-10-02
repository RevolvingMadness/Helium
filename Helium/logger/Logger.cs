namespace Helium.logger
{
    class Logger
    {
        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[INFO] ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}