using Hexed.Core;
using Hexed.Extensions;
using Hexed.SDK.Base;
using Hexed.Wrappers;
using static Hexed.SDK.Objects.Enums;

namespace Hexed.Modules
{
    internal class BunnyHop
    {
        public static void Update()
        {
            if (!Config.BHop) return;

            if (NativeMethods.GetForegroundWindow() != GameManager.Memory.MainWindowHandle) return;

            if (!GameHelper.IsKeyDown(32)) return;

            BasePlayer pLocal = PlayerManager.GetLocalPlayer();
            if (pLocal == null) return;

            if (pLocal.GetFlags().Check(PlayerFlag.OnGround))
            {
                NativeMethods.SendMessage(GameManager.Memory.MainWindowHandle, 0x100, 0x20, 0x390000);
                NativeMethods.SendMessage(GameManager.Memory.MainWindowHandle, 0x101, 0x20, 0x390000);
            }
        }
    }
}
