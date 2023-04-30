using Hexed.Memory.Manager;
using Hexed.Memory;
using System.Numerics;
using Hexed.Wrappers;
using static Hexed.SDK.Objects.Structs;
using static Hexed.SDK.Objects.Enums;
using Hexed.Core;
using Hexed.SDK.Manager;

namespace Hexed.SDK.Base
{
    internal class BasePlayer : BaseEntity
    {
        private BaseBone[] cachedBones = null;

        public BasePlayer(IntPtr address) : base(address) { }

        public int GetHealth()
        {
            return BitConverter.ToInt32(readData, MemoryHandler.NetVars["m_iHealth"].ToInt32());
        }

        public Team GetTeam()
        {
            return (Team)BitConverter.ToInt32(readData, MemoryHandler.NetVars["m_iTeamNum"].ToInt32());
        }

        public Vector3 GetAimPunchAngle()
        {
            byte[] vecData = new byte[12];
            Buffer.BlockCopy(readData, MemoryHandler.NetVars["m_vecAimPunch"].ToInt32(), vecData, 0, 12);
            return ProcessMemory.BytesTo<Vector3>(vecData);
        }
        public Vector3 GetViewPunchAngle()
        {
            byte[] vecData = new byte[12];
            Buffer.BlockCopy(readData, MemoryHandler.NetVars["m_vecViewPunch"].ToInt32(), vecData, 0, 12);
            return ProcessMemory.BytesTo<Vector3>(vecData);
        }

        public Vector3 GetViewOffset()
        {
            byte[] vecData = new byte[12];
            Buffer.BlockCopy(readData, MemoryHandler.NetVars["m_vecViewOffset"].ToInt32(), vecData, 0, 12);
            return ProcessMemory.BytesTo<Vector3>(vecData);
        }

        public Vector3 GetEyePos()
        {
            return GetPosition() + GetViewOffset();
        }

        public PlayerFlag GetFlags()
        {
            return (PlayerFlag)BitConverter.ToInt32(readData, MemoryHandler.NetVars["m_fFlags"].ToInt32());
        }

        public BaseWeapon GetWeapon()
        {
            var entIndex = BitConverter.ToInt32(readData, MemoryHandler.NetVars["m_hActiveWeapon"].ToInt32()) & 0xFFF;
            var currentWeapon = EntityManager.EntityList.GetEntityByIndex(entIndex);
            return currentWeapon != null ? new BaseWeapon(currentWeapon.Address) : null;
        }

        public List<BaseWeapon> GetWeapons()
        {
            var ret = new List<BaseWeapon>();
            if (EntityManager.EntityList.Entities == null) return ret;
            for (var i = 0; i < 8; i++)
            {
                var entity = EntityManager.EntityList.GetEntityByIndex(BitConverter.ToInt32(readData, MemoryHandler.NetVars["m_hMyWeapons"].ToInt32() + i * 4) & 0xFFF);
                if (entity == null) continue;
                var weapon = new BaseWeapon(entity.Address);
                if (weapon == null) continue;
                if (weapon.IsSpecial() || weapon.IsGrenade() || weapon.IsKnife()) continue;
                ret.Add(weapon);
            }
            return ret;
        }

        private static IntPtr entList;
        public BaseViewmodel GetViewModel()
        {
            if (entList == IntPtr.Zero) entList = SignatureManager.GetEntityList();
            int entIndex = BitConverter.ToInt32(readData, MemoryHandler.NetVars["m_hViewModel"].ToInt32()) & 0xFFF;
            var currentViewmodel = MemoryHandler.Memory.Read<IntPtr>(entList + (entIndex * 0x10));
            return currentViewmodel != IntPtr.Zero ? new BaseViewmodel(currentViewmodel) : null;
        }

        public BaseBone[] GetBoneMatrix()
        {
            var boneMatrix = new IntPtr(BitConverter.ToInt32(readData, MemoryHandler.NetVars["m_dwBoneMatrix"].ToInt32()));
            if (cachedBones == null)
                cachedBones = MemoryHandler.Memory.ReadArray<BaseBone>(boneMatrix, 128);
            return cachedBones;
        }

        public Vector3 GetBonePos(int boneId)
        {
            return GetBoneMatrix()[boneId];
        }

        public int GetFieldOfView()
        {
            var m_iFOVStart = BitConverter.ToInt32(readData, MemoryHandler.NetVars["m_iFOVStart"].ToInt32());
            return m_iFOVStart;
        }

        public bool IsDefusing()
        {
            return BitConverter.ToBoolean(readData, MemoryHandler.NetVars["m_bIsDefusing"].ToInt32());
        }

        public BasePlayer GetPlayerInCrosshair()
        {
            return EntityManager.EntityList.Players.FirstOrDefault(p => p.GetIndex() == BitConverter.ToInt32(readData, MemoryHandler.NetVars["m_iCrosshairId"].ToInt32()));
        }

        public float FlashAlpha
        {
            get
            {
                return BitConverter.ToSingle(readData, MemoryHandler.NetVars["m_flFlashAlpha"].ToInt32());
            }
            set
            {
                MemoryHandler.Memory.Write(address.Add(MemoryHandler.NetVars["m_flFlashAlpha"]), value);
            }
        }

        public bool IsSpotted
        {
            get
            {
                return BitConverter.ToBoolean(readData, MemoryHandler.NetVars["m_bSpotted"].ToInt32());
            }
            set
            {
                MemoryHandler.Memory.Write(address.Add(MemoryHandler.NetVars["m_bSpotted"]), value);
            }
        }
    }
}
