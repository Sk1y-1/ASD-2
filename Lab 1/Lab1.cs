using System;

namespace Lab1
{
    public struct SeriesData
    {
        public double Term;
        public double Sum;

        public SeriesData(double term, double sum)
        {
            Term = term;
            Sum = sum;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.Write("x: ");
            if (!double.TryParse(Console.ReadLine(), out double x)) 
            return;

            Console.Write("n: ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n < 1) 
            return;

            Console.WriteLine($"Res 1: {Method1(1, n, x, 1, 0):F10}"); // tail recursion
            Console.WriteLine($"Res 2: {Method2(1, n, x, 1).Sum:F10}"); // recursion with struct
            Console.WriteLine($"Res 3: {Method3(1, n, x, 1):F10}"); // standard recursion
            Console.WriteLine($"Math.Cos: {Math.Cos(x):F10}");
        }

        static double Method1(int i, int n, double x, double currentTerm, double currentSum)
        {
            currentSum += currentTerm;
            if (i == n) return currentSum;
            double nextTerm = -currentTerm * (x * x) / (2 * i * (2 * i - 1));
            return Method1(i + 1, n, x, nextTerm, currentSum);
        }

        static SeriesData Method2(int i, int n, double x, double currentTerm)
        {
            if (i == n) return new SeriesData(currentTerm, currentTerm);
            double nextTerm = -currentTerm * (x * x) / (2 * i * (2 * i - 1));
            SeriesData nextData = Method2(i + 1, n, x, nextTerm);
            return new SeriesData(currentTerm, currentTerm + nextData.Sum);
        }

        static double Method3(int i, int n, double x, double currentTerm)
        {
            if (i == n) 
            return currentTerm;
            double nextTerm = -currentTerm * (x * x) / (2 * i * (2 * i - 1));
            return currentTerm + Method3(i + 1, n, x, nextTerm);
        }
    }
}