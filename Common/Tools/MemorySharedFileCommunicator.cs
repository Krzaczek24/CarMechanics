using System.IO.MemoryMappedFiles;
using System.IO;
using System.Runtime.InteropServices;

namespace Common.Tools
{
    public static class MemorySharedFileCommunicator
    {
        public static MemoryMappedViewAccessor GetWriter(string filePath, string mapName, long capacity)
        {
            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            var mmf = MemoryMappedFile.CreateFromFile(fileStream, mapName, capacity, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, false);
            var accessor = mmf.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Write);
            return accessor;
        }

        public static MemoryMappedViewAccessor GetReader(string filePath, string mapName, long capacity)
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Write);
            var mmf = MemoryMappedFile.CreateFromFile(fileStream, mapName, capacity, MemoryMappedFileAccess.Read, HandleInheritability.None, false);
            var accessor = mmf.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read);
            return accessor;
        }
    }

    public class MemorySharedFileWriter<T>(string filePath, string mapName) where T : struct
    {
        private MemoryMappedViewAccessor Accessor { get; } = MemorySharedFileCommunicator.GetWriter(filePath, mapName, Marshal.SizeOf<T>());

        public void Write(T data)
        {
            Accessor.Write(0, ref data);
        }
    }

    public class MemorySharedFileReader<T>(string filePath, string mapName) where T : struct
    {
        private MemoryMappedViewAccessor Accessor { get; } = MemorySharedFileCommunicator.GetReader(filePath, mapName, Marshal.SizeOf<T>());

        public T Read()
        {
            Accessor.Read(0, out T data);
            return data;
        }
    }
}
