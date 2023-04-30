using Hexed.Memory.Manager;
using static Hexed.SDK.Objects.Structs;
using Hexed.Core;

namespace Hexed.SDK.Manager
{
    internal static class GlowManager
    {
        private static IntPtr glowAddress;
        public static IntPtr GetGlowBase()
        {
            while (glowAddress == IntPtr.Zero)
            {
                glowAddress = SignatureManager.GetGlowObjectBase();
            }
            return glowAddress;
        }

        public static int ObjectCount
        {
            get
            {
                return MemoryHandler.Memory.Read<int>(GetGlowBase() + 0xC);
            }
        }

        public static GlowObject[] Objects
        {
            get
            {
                return MemoryHandler.Memory.ReadArray<GlowObject>(MemoryHandler.Memory.Read<IntPtr>(GetGlowBase()), ObjectCount);
            }
        }
    }
}
