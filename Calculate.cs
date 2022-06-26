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

                if (input[0] != '(')
                {
                    if (char.IsDigit(input[open_bracket_index - 1]))
                    {
                        input = input.Insert(open_bracket_index, "*");
                        open_bracket_index++;
                        close_bracket_index++;
                    }
                }
                if (input[input.Length - 1] != ')')
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
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(20 + 10, height / 10 * 2 + 2);
                Console.Write("Caution: Close all brackets");
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

            //arr = input.Insert(0, input[1] == '-' ? "0" : "").Split(binary_operations.ToCharArray()).Select(Double.Parse).ToArray();

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
                Console.SetCursorPosition(20 + 10, height / 10 * 2 + 2);
                Console.Write("Error: Value is too big");
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
            // arr {1, 2, 3, 6}  =>  {1, 6, 6} => {1,1} => {2}
            // symbols {+, *, /} =>  {+, /} => {+}  =>  {}
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
