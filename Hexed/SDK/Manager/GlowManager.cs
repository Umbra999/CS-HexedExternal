using Hexed.Memory.Manager;
using Hexed.Memory;
using static Hexed.SDK.Objects.Structs;

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
                return MemorySettings.Memory.Read<int>(GetGlowBase() + 0xC);
            }
        }

        public static GlowObject[] Objects
        {
            get
            {
                return MemorySettings.Memory.ReadArray<GlowObject>(MemorySettings.Memory.Read<IntPtr>(GetGlowBase()), ObjectCount);
            }
        }
    }
}
