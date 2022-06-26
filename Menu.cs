using System;
using System.Linq;

namespace MICalculator
{
    internal class Menu : MFunctions
    {
        public static void Init()
        {
            FixConsole();
            while (true)
            {
                Frame();
                SetCursor(0);
                Console.Write(input);
                SetCursor(current_symbol + 1);
                ReadInput();
                CheckBackspace();
                CheckError();
                CheckMove();
                Clamp();
                Console.Clear();
                Console.SetCursorPosition(19, 2);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Symbol count: " + symbol_count);
                Console.ForegroundColor = ConsoleColor.Green;
                Output(Calculate.Brackets(input));
            }
        }
        public static void Frame()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            for (int i = 0; i < (width / 10 * 6) + 1; i++)
            {
                Console.SetCursorPosition(width / 10 * 2 + i, height / 10);
                Console.Write("-");
            }

            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(width / 10 * 2, height / 10 + (i + 1));
                Console.WriteLine('|');
            }
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(width / 10 * 8, height / 10 + (i + 1));
                Console.WriteLine('|');
            }


            for (int i = 0; i < (width / 10 * 6) + 1; i++)
            {
                Console.SetCursorPosition(width / 10 * 2 + i, (height / 10 + 4));
                Console.Write("-");
            }

            for (int i = 0; i < height - 5; i++)
            {
                Console.SetCursorPosition(width / 10 * 2, (height / 10 * 2) + 2 + i);
                Console.WriteLine('|');
            }
            for (int i = 0; i < height - 5; i++)
            {
                Console.SetCursorPosition(width / 10 * 8, (height / 10 * 2) + 2 + i);
                Console.WriteLine('|');
            }

            for (int i = 0; i < (width / 10 * 6) + 1; i++)
            {
                Console.SetCursorPosition(width / 10 * 2 + i, (height / 10 * 9) + 8);
                Console.Write("-");
            }

            Commands();
            Console.CursorVisible = true;
        }
        protected static void Commands()
        {
            string[] commands = { "(а) + (b) - calculates the sum",
                                  "(а) - (b) - calculates the subtraction",
                                  "(а) * (b) - calculates the product",
                                  "(a) / (b) - calculates the division",
                                  "(a) ^ (b) - calculates the power",
                                  "f(a) - calculates number in the fibonacci sequence",
                                  "!(а) - calculates the factorial of the number",
                                  "You can use \"(\" and \")\" to form complex expression",
                                  "\\-------------------------------------------------/",
                                  "|    More info - https://youtu.be/dQw4w9WgXcQ     |",
                                  "/-------------------------------------------------\\ ",
                                  };
            for (int i = 0; i < commands.Length; i++)
            {
                Console.SetCursorPosition((width / 10 * 2) + 2, (height / 10 * 6) + i);
                Console.WriteLine(commands[i]);
            }

        }
    }
}