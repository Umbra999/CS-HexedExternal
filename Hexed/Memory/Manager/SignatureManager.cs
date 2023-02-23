namespace Hexed.Memory.Manager
{
    internal static class SignatureManager
    {
        public static IntPtr GetViewAngle()
        {
            return MemorySettings.Engine.Find("F3 0F 11 86 ? ? ? ? F3 0F 10 44 24 ? F3 0F 11 86", 4, 0, PatternScan.ScanMethod.Read);
        }

        public static IntPtr GetCrosshairId()
        {
            return MemorySettings.Client.Find("8B EC 81 EC ? ? ? ? 56 57 8B F9 C7 87", 14, 0, PatternScan.ScanMethod.Read);
        }

        public static IntPtr GetLineInSmoke()
        {
            return MemorySettings.Client.Find("83 EC 44 8B 15", 0, 0, PatternScan.ScanMethod.Add) - 3;
        }

        public static IntPtr GetRankRevealer()
        {
            return MemorySettings.Client.Find("55 8B EC 8B 0D ? ? ? ? 68 ? ? ? ?", 0, 0, PatternScan.ScanMethod.Add);
        }

        public static IntPtr GetGlowObjectBase()
        {
            return MemorySettings.Client.Find("A1 ? ? ? ? A8 01 75 4B", 1, 4, PatternScan.ScanMethod.Read);
        }

        public static IntPtr GetEntityList()
        {
            return MemorySettings.Client.Find("BB ? ? ? ? 83 FF 01 0F 8C ? ? ? ? 3B F8", 1, -0x10, PatternScan.ScanMethod.Read);
        }

        public static IntPtr GetWorldToViewMatrix()
        {
            return MemorySettings.Client.Find("0F 10 05 ? ? ? ? 8D 85 ? ? ? ? B9", 3, 0xB0, PatternScan.ScanMethod.Read);
        }

        public static IntPtr GetClientState()
        {
            return MemorySettings.Engine.Find("A1 ? ? ? ? 33 D2 6A 00 6A 00 33 C9 89 B0", 1, 0, PatternScan.ScanMethod.Read);
        }

        public static IntPtr GetCmdNumber()
        {
            return MemorySettings.Engine.Find("56 57 8B 81 ? ? ? ? 8B B1 ? ? ? ? 40 03 C6", 4, 0, PatternScan.ScanMethod.Read);
        }

        public static IntPtr GetSendPacket()
        {
            return MemorySettings.Engine.Find("01 8B 01 8B 40 10", 0, 0, PatternScan.ScanMethod.Add);
        }

        public static IntPtr GetLocalIndex()
        {
            return MemorySettings.Engine.Find("8B 80 00 00 00 00 40 C3", 2, 0, PatternScan.ScanMethod.Read);
        }

        public static IntPtr GetGameDir()
        {
            return MemorySettings.Engine.Find("68 00 00 00 00 8D 85 00 00 00 00 50 68 00 00 00 00 68", 1, 0, PatternScan.ScanMethod.Read);
        }

        public static IntPtr GetMapName()
        {
            return MemorySettings.Engine.Find("05 00 00 00 00 C3 CC CC CC CC CC CC CC 80 3D", 1, 0, PatternScan.ScanMethod.Read);
        }

        public static IntPtr GetDormantOffset()
        {
            return MemorySettings.Client.Find("88 9E 00 00 00 00 E8 00 00 00 00 53 8D 8E", 2, 0, PatternScan.ScanMethod.Read);
        }

        public static IntPtr GetSignOnState()
        {
            return MemorySettings.Engine.Find("83 B9 ? ? ? ? 06 0F 94 C0 C3", 2, 0, PatternScan.ScanMethod.Read);
        }

        public static IntPtr GetClientClassesHead()
        {
            return MemorySettings.Memory.Read<IntPtr>(MemorySettings.Client.Find("A1 ? ? ? ? C3 CC CC CC CC CC CC CC CC CC CC A1 ? ? ? ? B9", 1, 0, PatternScan.ScanMethod.Read));
        }
    }
}
