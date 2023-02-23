using Hexed.Core;
using Hexed.Memory;
using Hexed.SDK;
using Hexed.SDK.Base;
using Hexed.Wrappers;
using static Hexed.SDK.Objects.Enums;

namespace Hexed.Modules
{
    internal class Misc
    {
        public static void Bunnyhop()
        {
            if (!Config.BHop) return;

            if (NativeMethods.GetForegroundWindow() != MemorySettings.Memory.MainWindowHandle) return;
            if (!GameHelper.IsKeyDown(32)) return;
            if (SDKSettings.EntityList == null) return;
            if (SDKSettings.EntityList.Players == null || SDKSettings.EntityList.Players.Count < 1) return;

            BasePlayer pLocal = SDKSettings.EntityList.GetLocalPlayer();
            if (pLocal == null) return;

            if (pLocal.GetFlags().Check(PlayerFlag.OnGround))
            {
                NativeMethods.SendMessage(MemorySettings.Memory.MainWindowHandle, 0x100, 0x20, 0x390000);
                NativeMethods.SendMessage(MemorySettings.Memory.MainWindowHandle, 0x101, 0x20, 0x390000);
            }
        }

        public static void Radar()
        {
            if (!Config.Radar) return;

            if (SDKSettings.EntityList == null) return;
            if (SDKSettings.EntityList.Players == null || SDKSettings.EntityList.Players.Count < 1) return;

            BasePlayer pLocal = SDKSettings.EntityList.GetLocalPlayer();
            if (pLocal == null) return;

            foreach (BasePlayer player in SDKSettings.EntityList.Players.Where(p => p.GetTeam() != pLocal.GetTeam() && p != null))
            {
                player.IsSpotted = !player.IsDormant() || player.GetHealth() >= 1;
            }
        }
    }
}
