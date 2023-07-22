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
        private static IntPtr viewMatrix;


        public static IntPtr ClientState
        {
            get
            {
                while (clientState == IntPtr.Zero)
                {
                    Thread.Sleep(10);
                    clientState = SignatureManager.GetClientState();
                }
                return GameManager.Memory.Read<IntPtr>(clientState);
            }
        }

        public static bool IsInMenu
        {
            get
            {
                if (signOnState == IntPtr.Zero) signOnState = SignatureManager.GetSignOnState();
                return GameManager.Memory.Read<int>(ClientState.Add(signOnState)) == 0;
            }
        }

        public static bool IsInGame
        {
            get
            {
                if (signOnState == IntPtr.Zero)  signOnState = SignatureManager.GetSignOnState();
                var state = GameManager.Memory.Read<int>(ClientState.Add(signOnState));
                return state > 1 || state < 7;
            }
        }

        public static int LocalPlayerIndex
        {
            get
            {
                if (localIndex == IntPtr.Zero) localIndex = SignatureManager.GetLocalIndex();
                var index = GameManager.Memory.Read<int>(ClientState.Add(localIndex));
                return index;
            }
        }

        public static int DesiredCmdNumber
        {
            get
            {
                if (cmdNumber == IntPtr.Zero) cmdNumber = SignatureManager.GetCmdNumber();
                var num = GameManager.Memory.Read<int>(ClientState.Add(cmdNumber));
                return num;
            }
        }

        public static bool SendPacket
        {
            get
            {
                if (sendPacket == IntPtr.Zero) sendPacket = SignatureManager.GetSendPacket();
                return GameManager.Memory.Read<bool>(sendPacket);
            }
            set
            {
                if (sendPacket == IntPtr.Zero) sendPacket = SignatureManager.GetSendPacket();
                GameManager.Memory.Write(sendPacket, value);
            }
        }

        public static string MapName
        {
            get
            {
                if (mapName == IntPtr.Zero) mapName = SignatureManager.GetMapName();
                var name = GameManager.Memory.ReadString(ClientState.Add(mapName));
                return name;
            }
        }

        public static string GameDirectory
        {
            get
            {
                if (gameDir == IntPtr.Zero) gameDir = SignatureManager.GetGameDir();
                return GameManager.Memory.ReadString(gameDir);
            }
        }

        public static float[] ViewMatrix
        {
            get
            {
                if (viewMatrix == IntPtr.Zero) viewMatrix = SignatureManager.GetWorldToViewMatrix();

                float[] temp = new float[16];
                for (int i = 0; i < 16; i++)
                {
                    temp[i] = GameManager.Memory.Read<float>(viewMatrix + (i * 0x4));
                }
                    
                return temp;
            }
        }

        public static Vector3 ViewAngles
        {
            get
            {
                if (viewAngle == IntPtr.Zero) viewAngle = SignatureManager.GetViewAngle();
                return GameManager.Memory.Read<Vector3>(ClientState.Add(viewAngle));
            }
            set
            {
                if (viewAngle == IntPtr.Zero) viewAngle = SignatureManager.GetViewAngle();
                GameManager.Memory.Write(ClientState.Add(viewAngle), value);

            }
        }
    }
}
