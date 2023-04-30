using Hexed.Memory;

namespace Hexed.Core
{
    internal class MemoryHandler
    {
        public static Dictionary<string, IntPtr> NetVars = new();
        public static ProcessMemory Memory;
        public static PatternScan Client;
        public static PatternScan Engine;
    }
}
