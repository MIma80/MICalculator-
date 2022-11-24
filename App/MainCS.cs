namespace MICalculator.App
{
    internal class MainCS
    {
        static void Main(string[] args)
        {
            #if Windows
                App.Run(args.Length == 2 ? int.Parse(args[0]): 60, args.Length == 2 ? int.Parse(args[1]): 25);
            #else
                global::System.Console.WriteLine("This application only can be run on Windows");
            #endif
        }
    }
}
