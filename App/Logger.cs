using System;

namespace MICalculator.App
{
    static internal class Logger
    {
        static public void Log(string message, int left, int top, ConsoleColor color)
        {
            if (message is null)
                return;

            Console.SetCursorPosition(left, top);
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
