using Hexed.Memory.Manager;
using Hexed.Memory;
using Hexed.Wrappers;
using System.Numerics;

namespace Hexed.SDK
{
    internal static class EngineClient
    {
        private static IntPtr clientState;
        private static IntPtr signOnState;
        private static IntPtr viewAngle;
        private static IntPtr localIndex;
        private static IntPtr mapName;
        private static IntPtr cmdNumber;
        private static IntPtr gameDir;
        private static IntPtr sendPacket;
        //private static string previousMap;
        //private static BspFile cachedMap;


        public static IntPtr ClientState
        {
            get
            {
                while (clientState == IntPtr.Zero)
                {
                    Thread.Sleep(10);
                    clientState = SignatureManager.GetClientState();
                }
                return MemorySettings.Memory.Read<IntPtr>(clientState);
            }
        }

        public static bool IsInMenu
        {
            get
            {
                if (signOnState == IntPtr.Zero)
                    signOnState = SignatureManager.GetSignOnState();
                return MemorySettings.Memory.Read<int>(ClientState.Add(signOnState)) == 0;
            }
        }

        public static bool IsInGame
        {
            get
            {
                if (signOnState == IntPtr.Zero)
                    signOnState = SignatureManager.GetSignOnState();
                var state = MemorySettings.Memory.Read<int>(ClientState.Add(signOnState));
                return state > 1 || state < 7;
            }
        }

        public static int LocalPlayerIndex
        {
            get
            {
                if (localIndex == IntPtr.Zero)
                {
                    localIndex = SignatureManager.GetLocalIndex();
                }
                var index = MemorySettings.Memory.Read<int>(ClientState.Add(localIndex));
                return index;
            }
        }

        public static int DesiredCmdNumber
        {
            get
            {
                if (cmdNumber == IntPtr.Zero)
                {
                    cmdNumber = SignatureManager.GetCmdNumber();
                }
                var num = MemorySettings.Memory.Read<int>(ClientState.Add(cmdNumber));
                return num;
            }
        }

        public static bool SendPacket
        {
            get
            {
                if (sendPacket == IntPtr.Zero)
                {
                    sendPacket = SignatureManager.GetSendPacket();
                }
                return MemorySettings.Memory.Read<bool>(sendPacket);
            }
            set
            {
                if (sendPacket == IntPtr.Zero)
                {
                    sendPacket = SignatureManager.GetSendPacket();
                }
                MemorySettings.Memory.Write(sendPacket, value);
            }
        }

        public static string MapName
        {
            get
            {
                if (mapName == IntPtr.Zero)
                {
                    mapName = SignatureManager.GetMapName();
                }
                var name = MemorySettings.Memory.ReadString(ClientState.Add(mapName));
                return name;
            }
        }

        public static string GameDirectory
        {
            get
            {
                if (gameDir == IntPtr.Zero)
                {
                    gameDir = SignatureManager.GetGameDir();
                }
                return MemorySettings.Memory.ReadString(gameDir);
            }
        }

        //public static BspFile Map
        //{
        //    get
        //    {
        //        var currentMap = MapName;
        //        if ((previousMap != currentMap || cachedMap == null) && currentMap.EndsWith(".bsp"))
        //        {
        //            previousMap = currentMap;
        //            cachedMap = new BspFile($"{GameDirectory}\\{currentMap}");
        //        }
        //        return cachedMap;
        //    }
        //}

        public static Vector3 ViewAngles
        {
            get
            {
                if (viewAngle == IntPtr.Zero) viewAngle = SignatureManager.GetViewAngle();
                return MemorySettings.Memory.Read<Vector3>(ClientState.Add(viewAngle));
            }
            set
            {
                if (viewAngle == IntPtr.Zero) viewAngle = SignatureManager.GetViewAngle();
                MemorySettings.Memory.Write(ClientState.Add(viewAngle), value);

            }
        }
    }
}
