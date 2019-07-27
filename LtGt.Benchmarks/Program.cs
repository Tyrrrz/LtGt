using BenchmarkDotNet.Running;

namespace LtGt.Benchmarks
{
    public static class Program
    {
        public static void Main() => BenchmarkRunner.Run(typeof(Program).Assembly);
    }
}