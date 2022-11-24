using System;

namespace Input
{
    static internal class InputHandler
    {
        /// <summary>
        /// Reads one key input and adds it to the input string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="current_symbol_position"></param>
        internal static void ReadKeyInput(ref string input, ref int current_symbol_position)
        {
            input = input.Insert(current_symbol_position, InputHandler.ReadKeys());
        }

        private static string ReadKeys()
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if ((key.Modifiers & ConsoleModifiers.Shift) != 0)
            {
                switch (key.Key)
                {
                    case ConsoleKey.D0:
                        return ")";

                    case ConsoleKey.D6:
                        return "^";

                    case ConsoleKey.D8:
                        return "*";

                    case ConsoleKey.D9:
                        return "(";

                    case ConsoleKey.OemPlus:
                        return "+";

                    default:
                        return "?";
                }
            }
            else
            {
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        return "<";

                    case ConsoleKey.RightArrow:
                        return ">";

                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        return "0";

                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return "1";

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return "2";

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return "3";

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        return "4";

                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        return "5";

                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        return "6";

                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        return "7";

                    case ConsoleKey.D8:
                    case ConsoleKey.NumPad8:
                        return "8";

                    case ConsoleKey.D9:
                    case ConsoleKey.NumPad9:
                        return "9";

                    case ConsoleKey.Add:
                        return "+";

                    case ConsoleKey.Multiply:
                        return "*";

                    case ConsoleKey.Divide:
                    case ConsoleKey.Oem2:
                        return "/";

                    case ConsoleKey.OemMinus:
                    case ConsoleKey.Subtract:
                        return "-";

                    case ConsoleKey.OemComma:
                        return ",";

                    case ConsoleKey.Backspace:
                        return "@";

                    default:
                        return "?";
                }
            }
        }
    }
}
