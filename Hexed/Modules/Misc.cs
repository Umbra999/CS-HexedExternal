using Hexed.Core;
using Hexed.SDK;
using Hexed.SDK.Base;
using Hexed.SDK.Manager;
using Hexed.Wrappers;
using static Hexed.SDK.Objects.Enums;

namespace Hexed.Modules
{
    internal class Misc
    {
        public static void Bunnyhop()
        {
            if (!Config.BHop) return;

            if (NativeMethods.GetForegroundWindow() != MemoryHandler.Memory.MainWindowHandle) return;
            if (!GameHelper.IsKeyDown(32)) return;
            if (EntityManager.EntityList == null) return;
            if (EntityManager.EntityList.Players == null || EntityManager.EntityList.Players.Count < 1) return;

            BasePlayer pLocal = EntityManager.EntityList.GetLocalPlayer();
            if (pLocal == null) return;

            if (pLocal.GetFlags().Check(PlayerFlag.OnGround))
            {
                NativeMethods.SendMessage(MemoryHandler.Memory.MainWindowHandle, 0x100, 0x20, 0x390000);
                NativeMethods.SendMessage(MemoryHandler.Memory.MainWindowHandle, 0x101, 0x20, 0x390000);
            }
        }

        public static void Radar()
        {
            if (!Config.Radar) return;

            if (EntityManager.EntityList == null) return;
            if (EntityManager.EntityList.Players == null || EntityManager.EntityList.Players.Count < 1) return;

            BasePlayer pLocal = EntityManager.EntityList.GetLocalPlayer();
            if (pLocal == null) return;

            foreach (BasePlayer player in EntityManager.EntityList.Players.Where(p => p.GetTeam() != pLocal.GetTeam() && p != null))
            {
                player.IsSpotted = !player.IsDormant() || player.GetHealth() >= 1;
            }
        }
    }
}
