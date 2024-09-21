using BenchmarkDotNet.Attributes;

#pragma warning disable CA1822
namespace Benchmarks
{
    [MemoryDiagnoser]
    public class SerializationBenchmark
    {
        private const int iterations = 100000;

        //[Benchmark]
        //public async Task SerializationUsingNewtonsoftLibrary()
        //{
        //    byte[] bytes = await GetBytesForNewtonsoftLibrary();
        //    for (int i = 0; i < iterations; i++)
        //    {
        //        using var stream = new MemoryStream(bytes);
        //        var obj = await JsonSerializer.DeserializeAsync<ControllerData>(stream);
        //    }
        //}

        //[Benchmark]
        //public async Task SerializationUsingCustomMethod()
        //{
        //    byte[] bytes = await GetBytesForCustomMethod();
        //    for (int i = 0; i < iterations; i++)
        //    {
        //        string message = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        //        var obj = await Task.FromResult(ControllerDataSerializer.Deserialize(message));
        //    }
        //}

        //private static async Task<byte[]> GetBytesForNewtonsoftLibrary()
        //{
        //    var obj = new ControllerData();
        //    using var stream = new MemoryStream();
        //    await JsonSerializer.SerializeAsync(stream, obj);
        //    byte[] bytes = stream.ToArray();
        //    return bytes;
        //}

        //private static async Task<byte[]> GetBytesForCustomMethod()
        //{
        //    var obj = new ControllerData();
        //    string str = ControllerDataSerializer.Serialize(obj);
        //    byte[] bytes = Encoding.UTF8.GetBytes(str);
        //    return await Task.FromResult(bytes);
        //}
    }
}
