namespace Hexed.Memory
{
    internal class MemorySettings
    {
        public static Dictionary<string, IntPtr> NetVars = new();
        public static ProcessMemory Memory;
        public static PatternScan Client;
        public static PatternScan Engine;
    }
}
