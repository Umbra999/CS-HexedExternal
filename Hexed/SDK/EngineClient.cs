using Hexed.Memory.Manager;
using Hexed.Wrappers;
using System.Numerics;
using Hexed.Core;

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
                return MemoryHandler.Memory.Read<IntPtr>(clientState);
            }
        }

        public static bool IsInMenu
        {
            get
            {
                if (signOnState == IntPtr.Zero)
                    signOnState = SignatureManager.GetSignOnState();
                return MemoryHandler.Memory.Read<int>(ClientState.Add(signOnState)) == 0;
            }
        }

        public static bool IsInGame
        {
            get
            {
                if (signOnState == IntPtr.Zero)
                    signOnState = SignatureManager.GetSignOnState();
                var state = MemoryHandler.Memory.Read<int>(ClientState.Add(signOnState));
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
                var index = MemoryHandler.Memory.Read<int>(ClientState.Add(localIndex));
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
                var num = MemoryHandler.Memory.Read<int>(ClientState.Add(cmdNumber));
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
                return MemoryHandler.Memory.Read<bool>(sendPacket);
            }
            set
            {
                if (sendPacket == IntPtr.Zero)
                {
                    sendPacket = SignatureManager.GetSendPacket();
                }
                MemoryHandler.Memory.Write(sendPacket, value);
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
                var name = MemoryHandler.Memory.ReadString(ClientState.Add(mapName));
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
                return MemoryHandler.Memory.ReadString(gameDir);
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
                return MemoryHandler.Memory.Read<Vector3>(ClientState.Add(viewAngle));
            }
            set
            {
                if (viewAngle == IntPtr.Zero) viewAngle = SignatureManager.GetViewAngle();
                MemoryHandler.Memory.Write(ClientState.Add(viewAngle), value);

            }
        }
    }
}
