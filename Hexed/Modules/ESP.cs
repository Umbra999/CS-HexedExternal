using Hexed.Core;
using Hexed.SDK;
using Hexed.SDK.Base;
using Hexed.SDK.Manager;
using System.Drawing;
using static Hexed.SDK.Objects.Structs;

namespace Hexed.Modules
{
    internal class ESP
    {
        public static void RenderPlayerESP()
        {
            if (!Config.ESP) return;

            if (EntityManager.EntityList == null) return;
            if (EntityManager.EntityList.Players == null || EntityManager.EntityList.Players.Count < 1) return;
            if (EntityManager.EntityList.Entities == null || EntityManager.EntityList.Entities.Count < 1) return;

            BasePlayer pLocal = EntityManager.EntityList.GetLocalPlayer();
            if (pLocal == null) return;

            GlowObject[] glowObjects = GlowManager.Objects;

            IntPtr glowBase = MemoryHandler.Memory.Read<IntPtr>(GlowManager.GetGlowBase());

            for (IntPtr index = 0; index < glowObjects.Length; index++)
            {
                GlowObject currentObject = glowObjects[index];
                if (currentObject.Entity == IntPtr.Zero) continue;
                BasePlayer pEntity = EntityManager.EntityList.Players.FirstOrDefault(p => p.Address == currentObject.Entity);
                if (pEntity == null) continue;
                if (pEntity.Address == pLocal.Address) continue;
                if (pEntity.IsDormant()) continue;
                if (pEntity.GetHealth() < 1) continue;
                if (pEntity.GetTeam() == pLocal.GetTeam()) continue;

                Color glowColor = Color.FromArgb(255, 0, 0, 255);

                if (pEntity.GetTeam() != pLocal.GetTeam())
                {
                    int healthRed = Math.Abs((int)(255 - (pEntity.GetHealth() * 2.55)));
                    glowColor = Color.FromArgb(255, healthRed, 255 - healthRed, 0);
                }

                LimitedGlowObject newObject = currentObject;

                newObject.A = glowColor.A / 255.0f;
                newObject.R = glowColor.R / 255.0f;
                newObject.G = glowColor.G / 255.0f;
                newObject.B = glowColor.B / 255.0f;

                newObject.m_bRenderWhenOccluded = 1;
                newObject.m_bRenderWhenUnoccluded = 0;
                newObject.m_bFullBloom = 0;

                MemoryHandler.Memory.Write(glowBase + 0x38 * index + 8, newObject);
            }

            
        }
    }
}
