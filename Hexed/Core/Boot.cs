using Hexed.HexedServer;
using Hexed.Memory;
using Hexed.Memory.Manager;
using Hexed.Modules;
using Hexed.SDK;
using Hexed.SDK.Manager;
using Hexed.Wrappers;
using System.Diagnostics;

namespace Hexed.Core
{
    internal class Boot
    {
        public static void Main()
        {
            Console.Title = Encryption.RandomString(16);

            Task Auth = Task.Run(ServerHandler.Init);
            Auth.Wait();

            Config.LoadConfig();
            Logger.Log($"Waiting for Process...");

            Process process = null;
            while (MemoryHandler.Memory == null || MemoryHandler.Client == null || MemoryHandler.Engine == null)
            {
                process = GameHelper.GetGameProcess();
                if (process != null && GameHelper.IsModuleLoaded(process, "serverbrowser.dll"))
                {
                    MemoryHandler.Memory = new ProcessMemory(process);
                    MemoryHandler.Client = new PatternScan(process, "client.dll");
                    MemoryHandler.Engine = new PatternScan(process, "engine.dll");
                }

                Thread.Sleep(1);
            }

            Logger.Log($"Attached to {process.ProcessName} [{process.Id}]");

            Logger.LogDebug("Hooked Modules:");
            Logger.LogDebug("  \tclient.dll \t\t| 0x" + MemoryHandler.Memory["client.dll"].BaseAddress.ToString("X").PadLeft(8, '0') + "\t| " + Wrappers.Math.ByteSizeToString(MemoryHandler.Memory["client.dll"].ModuleMemorySize));
            Logger.LogDebug("  \tengine.dll \t\t| 0x" + MemoryHandler.Memory["engine.dll"].BaseAddress.ToString("X").PadLeft(8, '0') + "\t| " + Wrappers.Math.ByteSizeToString(MemoryHandler.Memory["engine.dll"].ModuleMemorySize));

            IntPtr attribute = NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_Item").Add(NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_AttributeManager"));
            MemoryHandler.NetVars.Add("m_vecAimPunch", NetvarManager.GetOffset("DT_BasePlayer", "m_Local").Add(NetvarManager.GetOffset("DT_BasePlayer", "m_aimPunchAngle")));
            MemoryHandler.NetVars.Add("m_ItemDefIndex", attribute.Add(NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_iItemDefinitionIndex")));
            MemoryHandler.NetVars.Add("m_vecOrigin", NetvarManager.GetOffset("DT_BasePlayer", "m_vecOrigin"));
            MemoryHandler.NetVars.Add("m_iHealth", NetvarManager.GetOffset("DT_BasePlayer", "m_iHealth"));
            MemoryHandler.NetVars.Add("m_iTeamNum", NetvarManager.GetOffset("DT_BasePlayer", "m_iTeamNum"));
            MemoryHandler.NetVars.Add("m_vecViewOffset", NetvarManager.GetOffset("DT_BasePlayer", "m_vecViewOffset[0]"));
            MemoryHandler.NetVars.Add("m_dwIndex", new IntPtr(0x64));
            MemoryHandler.NetVars.Add("m_dwBoneMatrix", NetvarManager.GetOffset("DT_BaseAnimating", "m_nForceBone") + 0x1C);
            MemoryHandler.NetVars.Add("m_hActiveWeapon", NetvarManager.GetOffset("DT_BasePlayer", "m_hActiveWeapon"));
            MemoryHandler.NetVars.Add("m_hViewModel", NetvarManager.GetOffset("DT_BasePlayer", "m_hViewModel[0]"));
            MemoryHandler.NetVars.Add("m_hOwner", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_hOwner"));
            MemoryHandler.NetVars.Add("m_iState", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_iState"));
            MemoryHandler.NetVars.Add("m_nModelIndex", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_nModelIndex"));
            MemoryHandler.NetVars.Add("m_bDormant", SignatureManager.GetDormantOffset());
            MemoryHandler.NetVars.Add("m_nPaintKit", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_nFallbackPaintKit"));
            MemoryHandler.NetVars.Add("m_flWear", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_flFallbackWear"));
            MemoryHandler.NetVars.Add("m_iEntQuality", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_iEntityQuality"));
            MemoryHandler.NetVars.Add("m_iItemIDHigh", attribute.Add(NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_iItemIDHigh")));
            MemoryHandler.NetVars.Add("m_flFlashAlpha", NetvarManager.GetOffset("DT_CSPlayer", "m_flFlashMaxAlpha"));
            MemoryHandler.NetVars.Add("m_iFOVStart", NetvarManager.GetOffset("DT_CSPlayer", "m_iFOVStart"));
            MemoryHandler.NetVars.Add("m_bIsDefusing", NetvarManager.GetOffset("DT_CSPlayer", "m_bIsDefusing"));
            MemoryHandler.NetVars.Add("m_fFlags", NetvarManager.GetOffset("DT_CSPlayer", "m_fFlags"));
            MemoryHandler.NetVars.Add("m_bSpotted", NetvarManager.GetOffset("DT_CSPlayer", "m_bSpotted"));
            MemoryHandler.NetVars.Add("m_iCrosshairId", NetvarManager.GetOffset("DT_CSPlayer", "m_bHasDefuser") + 92);
            MemoryHandler.NetVars.Add("m_hMyWeapons", NetvarManager.GetOffset("DT_CSPlayer", "m_hMyWeapons"));
            MemoryHandler.NetVars.Add("m_hWeapon", NetvarManager.GetOffset("DT_BaseViewModel", "m_hWeapon"));

            MemoryHandler.NetVars.Sort();

            while (!process.HasExited)
            {
                if (EngineClient.IsInGame)
                {
                    EntityManager.UpdateEntities();
                    EntityManager.UpdatePlayers();
                    ESP.RenderPlayerESP();
                    Misc.Bunnyhop();
                    Misc.Radar();
                }

                Thread.Sleep(1);
            }
        }
    }
}
