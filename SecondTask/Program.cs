using System;
using System.IO;
using System.Threading.Tasks;

namespace SecondTask
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int factorialResult = 0;
            int countDigitsResult = 0;
            int sumOfDigitsResult = 0;

            Parallel.Invoke(
                async () => factorialResult = await FindFactorialAsync(5),
                async () => countDigitsResult = await CountDigitsAsync(11111),
                async () => sumOfDigitsResult = await SumOfDigitsAsync(11112),
                () => WriteTableToFile("multiplication_table.txt", GenerateMultiplicationTable(5, 8))
            );

            Console.WriteLine($"Factorial: {factorialResult}");
            Console.WriteLine($"Count of digits: {countDigitsResult}");
            Console.WriteLine($"Sum of digits: {sumOfDigitsResult}");

            Console.ReadKey();
        }

        static Task<int> FindFactorialAsync(int number)
        {
            return Task.Run(() =>
            {
                int factorial = 1;

                for (int i = 1; i <= number; i++)
                {
                    factorial *= i;
                }

                return factorial;
            });
        }

        static Task<int> CountDigitsAsync(int number)
        {
            return Task.Run(() =>
            {
                int count = 0;

                while (number > 0)
                {
                    number = number / 10;
                    count++;
                }

                return count;
            });
        }

        static Task<int> SumOfDigitsAsync(int number)
        {
            return Task.Run(() =>
            {
                int sum = 0;

                while (number > 0)
                {
                    sum += number % 10;
                    number = number / 10;
                }

                return sum;
            });
        }

        static string GenerateMultiplicationTable(int number, int limit)
        {
            using (StringWriter sw = new StringWriter())
            {
                sw.WriteLine($"Multiplication table for {number}:");
                for (int i = 1; i <= limit; i++)
                {
                    int result = number * i;
                    sw.WriteLine($"{number} x {i} = {result}");
                }
                sw.WriteLine();
                return sw.ToString();
            }
        }

        static void WriteTableToFile(string filePath, string table)
        {
            lock (filePath)
            {
                File.WriteAllText(filePath, table);
            }
        }
    }
}
