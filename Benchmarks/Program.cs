using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using KrzaqTools.Extensions;
using System.Reflection;

namespace Benchmarks
{
    internal class Program
    {
        private const int listIndexShift = 1;

        static void Main()
        {
            Type benchmark = ChooseBenchmark(GetBenchmarks());
            PrintBenchmarkHeader(benchmark);
            var summary = BenchmarkRunner.Run(benchmark);
        }

        private static List<Type> GetBenchmarks()
        {
            static bool IsBenchmark(Type type) => type.IsClass && !type.IsStatic() && type.HasAttribute<MemoryDiagnoserAttribute>();
            return [.. typeof(Program).Assembly.GetTypes().Where(IsBenchmark).OrderBy(t => t.Name)];
        }

        private static Type ChooseBenchmark(IList<Type> benchmarks)
        {
            while (true)
            {
                PrintChoiceMenu(benchmarks);
                char @char = Console.ReadKey().KeyChar;
                if (int.TryParse([@char], out int digit) && digit.IsBetween(listIndexShift, benchmarks.Count))
                    return benchmarks[digit - listIndexShift];
            }
        }

        private static void PrintChoiceMenu(IList<Type> benchmarks)
        {
            Console.Clear();
            Console.WriteLine("Choose benchmark to run:");
            foreach (var (@class, index) in benchmarks.WithIndex())
            {
                Console.WriteLine($"[{index + listIndexShift}] {@class.Name}");
            }
            Console.Write("Your choice: ");
        }

        private static void PrintBenchmarkHeader(Type benchmark)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(benchmark.Name);
            Console.WriteLine(string.Empty.PadRight(benchmark.Name.Length, '='));
        }
    }

    public static class TypeExtension
    {
        public static bool IsStatic(this Type type) => type.IsAbstract && type.IsSealed;
        public static bool HasAttribute<T>(this Type type) where T : Attribute => type.GetCustomAttribute<T>() != null;
    }
}
