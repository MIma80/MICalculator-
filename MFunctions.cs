using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace MICalculator
{
    internal class MFunctions
    {
        protected static string input = string.Empty;
        protected static int current_symbol = -1;
        protected static int symbol_count = 0;

        protected static string binary_operations = "+/*^";
        protected static string unary_operations = "!f";

        protected const int height = 25;
        protected const int width = 60;

        #region Import&&Consts
        protected const int SC_MINIMIZE = 0xF020;
        protected const int SC_MAXIMIZE = 0xF030;
        protected const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        #endregion
        private static extern IntPtr GetConsoleWindow();
        protected static void Clamp()
        {
            if (symbol_count > width - 5)
            {
                Delete(1, 0);
            }
        }
        protected static void Clear()
        {
            Console.Clear();
            Menu.Frame();
            current_symbol = -1;
            input = string.Empty;
            symbol_count = 0;
        }
        protected static void SetFixedConsole()
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, 0);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, 0);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, 0);

            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
        }
        protected static string ReadKeys()
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if ((key.Modifiers & ConsoleModifiers.Shift) != 0)
            {
                switch (key.Key)
                {

                    case ConsoleKey.D0:
                        {
                            return ")";
                        }

                    case ConsoleKey.D1:
                        {
                            return "!";
                        }

                    case ConsoleKey.D6:
                        {
                            return "^";
                        }

                    case ConsoleKey.D8:
                        {
                            return "*";
                        }

                    case ConsoleKey.D9:
                        {
                            return "(";
                        }
                    
                    case ConsoleKey.OemPlus:
                        {
                            return "+";
                        }
                    
                    default:
                        {
                            return "#";
                        }
                }
            }
            else
            {
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        {
                            return "<";
                        }
                    case ConsoleKey.RightArrow:
                        {
                            return ">";
                        }
                        case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        {
                            return "0";
                        }

                    case ConsoleKey.F:
                        {
                            return "f";
                        }
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            return "1";
                        }
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            return "2";
                        }
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        {
                            return "3";
                        }
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        {
                            return "4";
                        }

                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        {
                            return "5";
                        }

                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        {
                            return "6";
                        }

                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        {
                            return "7";
                        }

                    case ConsoleKey.D8:
                    case ConsoleKey.NumPad8:
                        {
                            return "8";
                        }

                    case ConsoleKey.D9:
                    case ConsoleKey.NumPad9:
                        {
                            return "9";
                        }               

                    case ConsoleKey.Add:
                        {
                            return "+";
                        }

                    case ConsoleKey.Multiply:
                        {
                            return "*";
                        }
                    

                    case ConsoleKey.Divide:
                    case ConsoleKey.Oem2:
                        {
                            return "/";
                        }

                    case ConsoleKey.OemMinus:
                    case ConsoleKey.Subtract:
                        {
                            return "-";
                        }

                    case ConsoleKey.OemComma:
                        {
                           return ",";
                        }
                    case ConsoleKey.Backspace:
                        {
                           return "@";
                        }
                    default:
                        {
                           return "#";
                        }
                }
            }
        }      
        protected static void ReadInput()
        {
            current_symbol++;
            input = input.Insert(current_symbol, ReadKeys());
            symbol_count++;
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
                Console.Clear();
            }
        }
        protected static void CheckBackspace()
        {
            if (input.Contains("@"))
            {
                Delete(2, 1);
            }
        }
        protected static void CheckError()
        {
            if (input.Contains("#"))
            {
                input = input.Remove(current_symbol, 1);
                Console.Clear();
                current_symbol--;
                symbol_count--;
            }

            if (current_symbol == -1)
                return;

            unary_operations = unary_operations.Insert(0, "-");
            if (input.Length >= 2 && current_symbol != 0)
                if (binary_operations.Contains(input[current_symbol]) && binary_operations.Contains(input[current_symbol - 1]) ||
                    unary_operations.Contains(input[current_symbol]) && unary_operations.Contains(input[current_symbol - 1]))
                {
                    Delete(1, 0);
                    Console.Clear();
                    Menu.Frame();
                    SetCursor(current_symbol);
                }
            if (input.Length >= 2 && current_symbol != input.Length - 1)
                if (binary_operations.Contains(input[current_symbol]) && binary_operations.Contains(input[current_symbol + 1]) ||
                    unary_operations.Contains(input[current_symbol]) && unary_operations.Contains(input[current_symbol + 1]))
                {
                    Delete(1, 0);
                    Console.Clear();
                    Menu.Frame();
                    SetCursor(current_symbol);
                }
            unary_operations = unary_operations.Remove(0, 1);
        }
        protected static void SetCursor(int position)
        {
            Console.SetCursorPosition(3 + position, 2);
        }
        protected static void CheckMove()
        {
            if (input.Contains("<"))
            {
                symbol_count--;
                if (current_symbol == 0)
                {
                    input = input.Remove(current_symbol, 1);
                    current_symbol--;
                }
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
                {
                    input = input.Remove(current_symbol, 1);
                    current_symbol--;
                }
                else
                {
                    input = input.Remove(current_symbol, 1);
                    SetCursor(current_symbol - 1);
                }
            }
        }
        protected static void Output(string result)
        {
            if (result != string.Empty)
            {
                if (Calculate.Brackets(input).Length + input.Length + 3 < width - 3)
                    SetCursor(input.Length);
                else Console.SetCursorPosition(width - 1 - result.Length - 4, 3);

                Console.ForegroundColor = ConsoleColor.Blue; 
                Console.Write("  = ");
                Console.Write(result);
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }
    }
}
