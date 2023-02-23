using Hexed.HexedServer;
using Hexed.Memory;
using Hexed.Memory.Manager;
using Hexed.Modules;
using Hexed.SDK.Manager;
using Hexed.Wrappers;
using System.Diagnostics;

namespace Hexed.Core
{
    internal class Main
    {
        public static void Boot()
        {
            Console.Title = Encryption.RandomString(16);

            var task = Task.Run(() => ServerHandler.Init());
            task.Wait();

            Logger.Log($"Waiting for Process...");

            while (MemorySettings.Memory == null)
            {
                Process process = GameHelper.GetGameProcess();
                if (process != null && GameHelper.IsModuleLoaded(process, "serverbrowser.dll"))
                {
                    MemorySettings.Memory = new ProcessMemory(process);
                    MemorySettings.Client = new PatternScan(process, "client.dll");
                    MemorySettings.Engine = new PatternScan(process, "engine.dll");
                }

                Thread.Sleep(1);
            }

            Logger.LogDebug("Hooked Modules:");
            Logger.LogDebug("  \tclient.dll \t\t| 0x" + MemorySettings.Memory["client.dll"].BaseAddress.ToString("X").PadLeft(8, '0') + "\t| " + Wrappers.Math.ByteSizeToString(MemorySettings.Memory["client.dll"].ModuleMemorySize));
            Logger.LogDebug("  \tengine.dll \t\t| 0x" + MemorySettings.Memory["engine.dll"].BaseAddress.ToString("X").PadLeft(8, '0') + "\t| " + Wrappers.Math.ByteSizeToString(MemorySettings.Memory["engine.dll"].ModuleMemorySize));

            IntPtr attribute = NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_Item").Add(NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_AttributeManager"));
            MemorySettings.NetVars.Add("m_vecAimPunch", NetvarManager.GetOffset("DT_BasePlayer", "m_Local").Add(NetvarManager.GetOffset("DT_BasePlayer", "m_aimPunchAngle")));
            MemorySettings.NetVars.Add("m_ItemDefIndex", attribute.Add(NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_iItemDefinitionIndex")));
            MemorySettings.NetVars.Add("m_vecOrigin", NetvarManager.GetOffset("DT_BasePlayer", "m_vecOrigin"));
            MemorySettings.NetVars.Add("m_iHealth", NetvarManager.GetOffset("DT_BasePlayer", "m_iHealth"));
            MemorySettings.NetVars.Add("m_iTeamNum", NetvarManager.GetOffset("DT_BasePlayer", "m_iTeamNum"));
            MemorySettings.NetVars.Add("m_vecViewOffset", NetvarManager.GetOffset("DT_BasePlayer", "m_vecViewOffset[0]"));
            MemorySettings.NetVars.Add("m_dwIndex", new IntPtr(0x64));
            MemorySettings.NetVars.Add("m_dwBoneMatrix", NetvarManager.GetOffset("DT_BaseAnimating", "m_nForceBone") + 0x1C);
            MemorySettings.NetVars.Add("m_hActiveWeapon", NetvarManager.GetOffset("DT_BasePlayer", "m_hActiveWeapon"));
            MemorySettings.NetVars.Add("m_hViewModel", NetvarManager.GetOffset("DT_BasePlayer", "m_hViewModel[0]"));
            MemorySettings.NetVars.Add("m_hOwner", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_hOwner"));
            MemorySettings.NetVars.Add("m_iState", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_iState"));
            MemorySettings.NetVars.Add("m_nModelIndex", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_nModelIndex"));
            MemorySettings.NetVars.Add("m_bDormant", SignatureManager.GetDormantOffset());
            MemorySettings.NetVars.Add("m_nPaintKit", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_nFallbackPaintKit"));
            MemorySettings.NetVars.Add("m_flWear", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_flFallbackWear"));
            MemorySettings.NetVars.Add("m_iEntQuality", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_iEntityQuality"));
            MemorySettings.NetVars.Add("m_iItemIDHigh", attribute.Add(NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_iItemIDHigh")));
            MemorySettings.NetVars.Add("m_flFlashAlpha", NetvarManager.GetOffset("DT_CSPlayer", "m_flFlashMaxAlpha"));
            MemorySettings.NetVars.Add("m_iFOVStart", NetvarManager.GetOffset("DT_CSPlayer", "m_iFOVStart"));
            MemorySettings.NetVars.Add("m_bIsDefusing", NetvarManager.GetOffset("DT_CSPlayer", "m_bIsDefusing"));
            MemorySettings.NetVars.Add("m_fFlags", NetvarManager.GetOffset("DT_CSPlayer", "m_fFlags"));
            MemorySettings.NetVars.Add("m_bSpotted", NetvarManager.GetOffset("DT_CSPlayer", "m_bSpotted"));
            MemorySettings.NetVars.Add("m_iCrosshairId", NetvarManager.GetOffset("DT_CSPlayer", "m_bHasDefuser") + 92);
            MemorySettings.NetVars.Add("m_hMyWeapons", NetvarManager.GetOffset("DT_CSPlayer", "m_hMyWeapons"));
            MemorySettings.NetVars.Add("m_hWeapon", NetvarManager.GetOffset("DT_BaseViewModel", "m_hWeapon"));

            MemorySettings.NetVars.Sort();

            Logger.Log("Attached to Process");

            while (true)
            {
                EntityManager.UpdateEntities();
                EntityManager.UpdatePlayers();
                ESP.RenderPlayerESP();
                Misc.Bunnyhop();
                Misc.Radar();

                Thread.Sleep(1);
            }
        }
    }
}
