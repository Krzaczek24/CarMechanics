namespace Common.Constants
{
    public static class Settings
    {
        public static class MemorySharedFile
        {
#if DEBUG
            public const string TEMP_FILE_PATH = @"c:\temp\data.tmp";
#else
            public const string TEMP_FILE_PATH = @"data.tmp";
#endif
            public const string CONTROLLER_DATA_MAP = "E4C67153-E6E7-4BE9-9D98-17C36A76CD62";
        }
    }
}
