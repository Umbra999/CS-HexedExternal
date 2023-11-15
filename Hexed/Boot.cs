using Hexed.Core;
using Hexed.Extensions;
using Hexed.Memory;
using Hexed.Memory.Manager;
using Hexed.Modules;
using Hexed.Wrappers;
using System.Diagnostics;

namespace Hexed
{
    internal class Boot
    {
        public static void Main()
        {
            Logger.Log($"Waiting for Process...");

            Process process = null;
            while (GameManager.Memory == null || GameManager.Client == null || GameManager.Engine == null)
            {
                process = GameHelper.GetGameProcess();
                if (process != null && GameHelper.IsModuleLoaded(process, "serverbrowser.dll"))
                {
                    GameManager.Memory = new ProcessMemory(process);
                    GameManager.Client = new PatternScan(process, "client.dll");
                    GameManager.Engine = new PatternScan(process, "engine.dll");
                }

                Thread.Sleep(1);
            }

            Logger.Log($"Attached to {process.ProcessName} [{process.Id}]");

            Logger.LogDebug("Hooked Modules:");
            Logger.LogDebug("client.dll \t| 0x" + GameManager.Memory["client.dll"].BaseAddress.ToString("X").PadLeft(8, '0') + "\t| " + Wrappers.Math.ByteSizeToString(GameManager.Memory["client.dll"].ModuleMemorySize));
            Logger.LogDebug("engine.dll \t| 0x" + GameManager.Memory["engine.dll"].BaseAddress.ToString("X").PadLeft(8, '0') + "\t| " + Wrappers.Math.ByteSizeToString(GameManager.Memory["engine.dll"].ModuleMemorySize));

            nint attribute = NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_Item").Add(NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_AttributeManager"));
            GameManager.NetVars.Add("m_vecAimPunch", NetvarManager.GetOffset("DT_BasePlayer", "m_Local").Add(NetvarManager.GetOffset("DT_BasePlayer", "m_aimPunchAngle")));
            GameManager.NetVars.Add("m_ItemDefIndex", attribute.Add(NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_iItemDefinitionIndex")));
            GameManager.NetVars.Add("m_vecOrigin", NetvarManager.GetOffset("DT_BasePlayer", "m_vecOrigin"));
            GameManager.NetVars.Add("m_iHealth", NetvarManager.GetOffset("DT_BasePlayer", "m_iHealth"));
            GameManager.NetVars.Add("m_iTeamNum", NetvarManager.GetOffset("DT_BasePlayer", "m_iTeamNum"));
            GameManager.NetVars.Add("m_vecViewOffset", NetvarManager.GetOffset("DT_BasePlayer", "m_vecViewOffset[0]"));
            GameManager.NetVars.Add("m_dwIndex", new nint(0x64));
            GameManager.NetVars.Add("m_dwBoneMatrix", NetvarManager.GetOffset("DT_BaseAnimating", "m_nForceBone") + 0x1C);
            GameManager.NetVars.Add("m_hActiveWeapon", NetvarManager.GetOffset("DT_BasePlayer", "m_hActiveWeapon"));
            GameManager.NetVars.Add("m_hViewModel", NetvarManager.GetOffset("DT_BasePlayer", "m_hViewModel[0]"));
            GameManager.NetVars.Add("m_hOwner", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_hOwner"));
            GameManager.NetVars.Add("m_iState", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_iState"));
            GameManager.NetVars.Add("m_nModelIndex", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_nModelIndex"));
            GameManager.NetVars.Add("m_bDormant", SignatureManager.GetDormantOffset());
            GameManager.NetVars.Add("m_nPaintKit", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_nFallbackPaintKit"));
            GameManager.NetVars.Add("m_flWear", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_flFallbackWear"));
            GameManager.NetVars.Add("m_iEntQuality", NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_iEntityQuality"));
            GameManager.NetVars.Add("m_iItemIDHigh", attribute.Add(NetvarManager.GetOffset("DT_BaseCombatWeapon", "m_iItemIDHigh")));
            GameManager.NetVars.Add("m_flFlashAlpha", NetvarManager.GetOffset("DT_CSPlayer", "m_flFlashMaxAlpha"));
            GameManager.NetVars.Add("m_iFOVStart", NetvarManager.GetOffset("DT_CSPlayer", "m_iFOVStart"));
            GameManager.NetVars.Add("m_bIsDefusing", NetvarManager.GetOffset("DT_CSPlayer", "m_bIsDefusing"));
            GameManager.NetVars.Add("m_fFlags", NetvarManager.GetOffset("DT_CSPlayer", "m_fFlags"));
            GameManager.NetVars.Add("m_bSpotted", NetvarManager.GetOffset("DT_CSPlayer", "m_bSpotted"));
            GameManager.NetVars.Add("m_iCrosshairId", NetvarManager.GetOffset("DT_CSPlayer", "m_bHasDefuser") + 92);
            GameManager.NetVars.Add("m_hMyWeapons", NetvarManager.GetOffset("DT_CSPlayer", "m_hMyWeapons"));
            GameManager.NetVars.Add("m_hWeapon", NetvarManager.GetOffset("DT_BaseViewModel", "m_hWeapon"));

            GameManager.NetVars.Sort();

            Config.LoadConfig();
            GUI.Run();

            while (!process.HasExited)
            {
                if (ConnectionManager.IsInWorld())
                {
                    BunnyHop.Update();
                    Radar.Update();
                }

                Thread.Sleep(1);
            }
        }
    }
}
