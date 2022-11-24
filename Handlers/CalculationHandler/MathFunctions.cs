namespace MathF
{
    internal static class BinaryOperation
    {
        internal delegate double Operation(double first_number, double second_number);

        internal static double Sum(double first, double second) => first+second;
        internal static double Divide(double first, double second) => first / second;
        internal static double Multiply(double first, double second) => first * second;
    }
}
