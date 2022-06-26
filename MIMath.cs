using System;
using System.Linq;

namespace MICalculator
{
    internal static class MIMath
    {
        public delegate double simple_operation(double first_number, double second_number);
        public delegate long complex_operation(int number);

        public const double PI = 3.14159265359;
        public const double EXP = 2.71828;

        public static double Sum(double first, double second)
        {
            return first+second;
        }
        public static double Divide(double first, double second)
        {
            try
            {
                if (second == 0) throw new DivideByZeroException("Attempt to divide by zero");
                return first / second;
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }
        public static double Multiply(double first, double second)
        {
            return first * second;
        }
        public static long Factorial(int number)
        {
            long result = 2;
            for (int i = 3; i <= number; i++)
            {
                result *= i;
            }
            return result;
        }
        public static long Fib(int number)
        {
            if (number == 0)
                return 0;
            if (number == 1)
                return 0;
            if (number == 2)
                return 1;

            if(cache[number] == 0) cache[number] = Fib(number - 1) + Fib(number - 2);
            return cache[number];
        }
        private static long[] cache = new long[100];
    }
}
