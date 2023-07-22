using Hexed.Core;
using Hexed.Extensions;
using Hexed.SDK.Base;

namespace Hexed.Modules
{
    internal class Radar
    {
        public static void Update()
        {
            if (!Config.Radar) return;

            BasePlayer pLocal = PlayerManager.GetLocalPlayer();
            if (pLocal == null) return;

            foreach (BasePlayer player in PlayerManager.GetPlayers().Where(p => p.GetTeam() != pLocal.GetTeam() && p != null))
            {
                player.IsSpotted = !player.IsDormant() || player.GetHealth() >= 1;
            }
        }
    }
}
