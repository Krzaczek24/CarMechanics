using System.IO.MemoryMappedFiles;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Common.Tools
{
    public static class MemorySharedFileCommunicator
    {
        private record struct Settings(FileMode FileMode, FileAccess FileAccess, FileShare FileShare, MemoryMappedFileAccess MemoryMappedFileAccess, MemoryMappedFileAccess ViewAccessorFileAccess) { }
        private static readonly Settings WriterSettings = new(FileMode.Create, FileAccess.ReadWrite, FileShare.Read, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileAccess.Write);
        private static readonly Settings ReaderSettings = new(FileMode.Open, FileAccess.Read, FileShare.ReadWrite, MemoryMappedFileAccess.Read, MemoryMappedFileAccess.Read);

        public static MemoryMappedViewAccessor GetWriter(string filePath, string mapName, long capacity) => CreateMemoryMappedFileAccessor(filePath, mapName, capacity, WriterSettings);
        public static MemoryMappedViewAccessor GetReader(string filePath, string mapName, long capacity) => CreateMemoryMappedFileAccessor(filePath, mapName, capacity, ReaderSettings);

        private static MemoryMappedViewAccessor CreateMemoryMappedFileAccessor(string filePath, string mapName, long capacity, Settings settings)
        {
            MemoryMappedViewAccessor accessor;
            while (!TryCreateMemoryMappedFile(filePath, mapName, capacity, settings, out accessor))
                Thread.Sleep(100);
            return accessor;
        }

        private static bool TryCreateMemoryMappedFile(string filePath, string mapName, long capacity, Settings settings, out MemoryMappedViewAccessor accessor)
        {
            try
            {
                var fileStream = new FileStream(filePath, settings.FileMode, settings.FileAccess, settings.FileShare);
                var memoryFile = MemoryMappedFile.CreateFromFile(fileStream, mapName, capacity, settings.MemoryMappedFileAccess, HandleInheritability.None, false);
                accessor = memoryFile.CreateViewAccessor(0, 0, settings.ViewAccessorFileAccess);
                return true;
            }
            catch
            {
                accessor = null;
                return false;
            }
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
