using System;
using System.Runtime.InteropServices;
using Calculation;
using Input;

namespace MICalculator.App
{
    internal class App
    {
        #region Import&&Consts
        protected const int SC_MINIMIZE = 0xF020;
        protected const int SC_MAXIMIZE = 0xF030;
        protected const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        #endregion

        // Variables
        protected static string input = string.Empty;
        protected static int current_symbol = -1;
        protected static int symbol_count = 0;

        protected static int width;
        protected static int height;

        protected static readonly int min_width = 60;
        protected static readonly int min_height = 25;

        // Main
        public static void Run(int _width = 60, int _height = 25)
        {
            // Set console
            width = _width > min_width ? _width: min_width;
            height = _height > min_height ? _height : min_height;
            SetFixedConsole();

            while (true)
            {
                try
                {
                    // Setup
                    Frame();
                    SetCursor(0);
                    Console.Write(input);
                    SetCursor(current_symbol + 1);

                    // ReadInput
                    current_symbol++;
                    InputHandler.ReadKeyInput(ref input, ref current_symbol);
                    symbol_count++;

                    // Processing
                    Console.Clear();
                    CheckBackspace();
                    CheckMove();
                    Clamp();

                    string result = CalculationHandler.Calculate(input).ToString();
                    Logger.Log
                    (
                        message: $"  = {result}",
                        left: result.Length + input.Length + 6 < width - 3 ? 3 + input.Length : 3 + width - 1 - result.Length - 7,
                        top: result.Length + input.Length + 6 < width - 3 ? 2 : 3,
                        color: ConsoleColor.Blue
                    );                       
                }
                catch (IndexOutOfRangeException)
                {
                    Logger.Log("Write a mathematical expression", 2, 3, ConsoleColor.Yellow);
                }
                catch (Exception)
                {
                    Logger.Log("Invalid input", 2, 3, ConsoleColor.Red);
                }
            }
        }


        private static void SetCursor(int position)
        {
            Console.SetCursorPosition(3 + position, 2);
        }
        private static void SetFixedConsole()
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, 0);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, 0);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, 0);

            Console.Title = "Calculator";
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
        }
        

        // Processing
        private static void Clamp()
        {
            if (symbol_count > width - 5)
                Delete(1, 0);
        }
        private static void CheckBackspace()
        {
            if (input.Contains("@"))
            {
                Delete(2, 1);
            }
        }
        private static void CheckMove()
        {
            if (input.Contains("<"))
            {
                symbol_count--;
                if (current_symbol == 0)
                    input = input.Remove(current_symbol--, 1);
                else
                {
                    input = input.Remove(current_symbol, 1);
                    current_symbol -= 2;
                    SetCursor(current_symbol + 1);
                }
            }
            if (input.Contains(">"))
            {
                symbol_count--;
                if (current_symbol == input.Length - 1)
                    input = input.Remove(current_symbol--, 1);
                else
                {
                    input = input.Remove(current_symbol, 1);
                    SetCursor(current_symbol - 1);
                }
            }
        }
        private static void Delete(int count, int shiftpos)
        {
            if (current_symbol == 0)
            {
                symbol_count--;
                input = input.Remove(current_symbol, 1);
                current_symbol--;

            }
            else
            {
                symbol_count -= count;
                input = input.Remove(current_symbol - shiftpos, count);
                current_symbol -= count;
            }
        }


        // Interface
        private static void Frame()
        {
            char horizontal = '─';
            char vertical = '│';

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
           
            Console.WriteLine('┌');
            for (int i = 1; i < width - 1; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write(horizontal);
            }
            Console.WriteLine('┐');

            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(0, i + 1);
                Console.Write(vertical);
            }

            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(width - 1, i + 1);
                Console.Write(vertical);
            }

            Console.Write('└');
            for (int i = 1; i < width - 1; i++)
            {
                Console.SetCursorPosition(i, 4);
                Console.Write(horizontal);
            }
            Console.Write('┘');

            for (int i = 6; i < height; i++)
            {
                Console.SetCursorPosition(1, i - 1);
                Console.Write(vertical);
            }

            for (int i = 6; i < height; i++)
            {
                Console.SetCursorPosition(width - 2, i - 1);
                Console.Write(vertical);
            }

            Console.SetCursorPosition(1, height - 1);
            Console.Write('└');
            for (int i = 1; i < width - 3; i++)
            {
                Console.SetCursorPosition(i + 1, height - 1);
                Console.Write(horizontal);
            }
            Console.Write('┘');

            Commands();
            Console.CursorVisible = true;
        }
        private static void Commands()
        {
            string[] commands = { "(а) + (b) - calculates the sum",
                                  "(а) - (b) - calculates the subtraction",
                                  "(а) * (b) - calculates the product",
                                  "(a) / (b) - calculates the division",
                                  "(a) ^ (b) - calculates the power",
                                  "",
                                  "Use \"(\" and \")\" to form complex expression",
                                  "Use left and right arrows to move cursor",
                                  "Use backspace to delete symbols",
                                  };
            for (int i = 0; i < commands.Length; i++)
            {
                Console.SetCursorPosition(width / 6, (height / 6 + 4) + i);
                Console.WriteLine(commands[i]);
            }
        }
    }
}