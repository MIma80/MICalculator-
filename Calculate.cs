using System;
using System.Linq;

namespace MICalculator
{
    internal class Calculate : MFunctions
    {
        protected static double[] arr;
        protected static string symbols;

        public static string Brackets(string input)
        {
            try
            {
                if ((input.IndexOf('(') == -1) && (input.IndexOf(')') == -1))
                    return Compute(input);

                string temp = input.Substring(input.LastIndexOf('(') + 1);
                temp = temp.Remove(temp.IndexOf(')'));

                int open_bracket_index = input.LastIndexOf('(');
                int close_bracket_index = input.LastIndexOf('(') + temp.Length + 1;
                int bracket_symbol_count = close_bracket_index - open_bracket_index + 1;

                if (open_bracket_index != 0)
                {
                    if (char.IsDigit(input[open_bracket_index - 1]))
                    {
                        input = input.Insert(open_bracket_index, "*");
                        open_bracket_index++;
                        close_bracket_index++;
                    }
                }
                if (input.Length - 1 != close_bracket_index)
                {
                    if (char.IsDigit(input[close_bracket_index + 1]))
                    {
                        input = input.Insert(close_bracket_index + 1, "*");
                    }
                }

                input = input.Remove(open_bracket_index, bracket_symbol_count).Insert(open_bracket_index, Compute(temp));
                return Brackets(input);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
                string message = "Caution: Invalid input";
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(width / 2 - message.Length / 2, 3);
                Console.Write(message);
                Console.ForegroundColor = ConsoleColor.Green;
                return string.Empty;
            }
        }
        private static string Compute(string input)
        {
            if (string.IsNullOrEmpty(input) || binary_operations.Contains(input.Last()))
                return string.Empty;
            if (input.Replace("-", "+-").Count(x => (binary_operations.Contains(x) || unary_operations.Contains(x))) == 0)
                return input;

            input = input.Replace("-", "+-");

            for (int i = 0; i < input.Length - 1; i++)
            {
                if (binary_operations.Contains(input[i]) && binary_operations.Contains(input[i + 1]))
                {
                    input = input.Remove(i + 1, 1);
                }
            }

            string temp = binary_operations.Contains(input[0]) ? input.Remove(0, 1) : input;
            symbols = new String(temp.Where(x => (binary_operations.Contains(x) || unary_operations.Contains(x))).ToArray());
            temp = unary_operations.Contains(temp[0]) ? temp.Remove(0, 1) : temp;
            for (int i = 1; i < input.Length - 1; i++)
            {
                if (binary_operations.Contains(temp[i]) || unary_operations.Contains(temp[i]))
                {
                    while (binary_operations.Contains(temp[i]) || unary_operations.Contains(temp[i]))
                    {
                        temp = temp.Remove(i, 1);
                    }
                    temp = temp.Insert(i, " ");
                }
            }
            arr = temp.Split(' ').Select(Double.Parse).ToArray();

            Unary_Operation_Simplification('!', 21, MIMath.Factorial);
            Unary_Operation_Simplification('f', 94, MIMath.Fib);
            if (arr.Contains(double.MaxValue))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                string message = "Error: Value is too big";
                Console.SetCursorPosition(width / 2 - message.Length / 2, 3);
                Console.Write(message);
                Console.ForegroundColor = ConsoleColor.Green;
                return string.Empty;
            }
            Binary_Operation_Simplification('^', Math.Pow);
            Binary_Operation_Simplification('/', MIMath.Divide);
            Binary_Operation_Simplification('*', MIMath.Multiply);
            Binary_Operation_Simplification('+', MIMath.Sum);

            return arr[0].ToString();
        }
        private static void Unary_Operation_Simplification(char symbol, int clamp, MIMath.complex_operation complex_op)
        {
            while (symbols.IndexOf(symbol) != -1)
            {
                arr[symbols.IndexOf(symbol)] = arr[symbols.IndexOf(symbol)] >= clamp ? double.MaxValue : complex_op((int)arr[symbols.IndexOf(symbol)]);
                symbols = symbols.Remove(symbols.IndexOf(symbol), 1);
            }
        }
        private static void Binary_Operation_Simplification(char symbol, MIMath.simple_operation simple_op)
        {
            // arr {1, 2, 3, 6}  =>  {1, 2, 0.5} => {1,1} => {2}
            // symbols {+, *, /} =>  {+, *} => {+}  =>  {}
            while (symbols.IndexOf(symbol) != -1)
            {
                double locres = simple_op(arr[symbols.IndexOf(symbol)], arr[symbols.IndexOf(symbol) + 1]);
                arr = arr.Where((val, idx) => idx != symbols.IndexOf(symbol)).ToArray();
                arr[symbols.IndexOf(symbol)] = locres;
                symbols = symbols.Remove(symbols.IndexOf(symbol), 1);
            }
        }
    }
}
