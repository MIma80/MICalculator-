using System;
using System.Linq;
using MathF;

namespace Calculation
{
    static internal class CalculationHandler
    {
        private static double[] arr;
        private static string symbols;

        private static string binary_operations = "+/*^";

        /// <summary>
        /// Calculates the expression.
        /// The expression must not contain spaces and must consist of numbers and mathematical characters.
        /// </summary>
        /// <param name="input">Expression to calculate</param>
        /// <returns>Returns result of expression.</returns>
        internal static double Calculate(string input)
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
                if (char.IsDigit(input[close_bracket_index + 1]))
                    input = input.Insert(close_bracket_index + 1, "*");

            input = input.Remove(open_bracket_index, bracket_symbol_count).Insert(open_bracket_index, Compute(temp).ToString());
            return Calculate(input);
        }
        private static double Compute(string input)
        {
            input = input.Replace("-", "+-");

            string temp = binary_operations.Contains(input[0]) ? input.Remove(0, 1) : input;
            symbols = new String(temp.Where(x => (binary_operations.Contains(x))).ToArray());
            for (int i = 1; i < input.Length - 1; i++)
            {
                if (binary_operations.Contains(temp[i]))
                {
                    while (binary_operations.Contains(temp[i]))
                    {
                        temp = temp.Remove(i, 1);
                    }
                    temp = temp.Insert(i, " ");
                }
            }
            arr = temp.Split(' ').Select(Double.Parse).ToArray();

            // Priority from highest to lowest 
            // First -> The highest
            // Last -> The lowest

            Binary_Operation_Simplification('^', Math.Pow); 
            Binary_Operation_Simplification('/', BinaryOperation.Divide); 
            Binary_Operation_Simplification('*', BinaryOperation.Multiply); 
            Binary_Operation_Simplification('+', BinaryOperation.Sum);

            return arr[0];
        }
        private static void Binary_Operation_Simplification(char symbol, BinaryOperation.Operation binary_op)
        {
            // 1 + 2 * 3 / 6 = ?
            /*
             * arr {1, 2, 3, 6}  =>  {1, 2, 0.5} => {1,1} => {2}
             * symbols {+, *, /} =>  {+, *} => {+}  =>  {}
            */

            checked
            {
                while (symbols.IndexOf(symbol) != -1)
                {
                    double res = binary_op(arr[symbols.IndexOf(symbol)], arr[symbols.IndexOf(symbol) + 1]);
                    arr = arr.Where((val, idx) => idx != symbols.IndexOf(symbol)).ToArray();
                    arr[symbols.IndexOf(symbol)] = res;
                    symbols = symbols.Remove(symbols.IndexOf(symbol), 1);
                }
            }
        }
    }
}
