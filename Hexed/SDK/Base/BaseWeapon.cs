﻿using Hexed.Memory;
using Hexed.Wrappers;
using static Hexed.SDK.Objects.Enums;

namespace Hexed.SDK.Base
{
    internal class BaseWeapon : BaseEntity
    {
        public BaseWeapon(IntPtr address) : base(address) { }

        public ItemDefinitionIndex GetItemDefinitionIndex()
        {
            return (ItemDefinitionIndex)BitConverter.ToInt32(readData, MemorySettings.NetVars["m_ItemDefIndex"].ToInt32());
        }

        public int PaintKit
        {
            get
            {
                return BitConverter.ToInt32(readData, MemorySettings.NetVars["m_nPaintKit"].ToInt32());
            }
            set
            {
                MemorySettings.Memory.Write(address.Add(MemorySettings.NetVars["m_nPaintKit"]), value);
            }
        }

        public int ItemIDHigh
        {
            get
            {
                return BitConverter.ToInt32(readData, MemorySettings.NetVars["m_iItemIDHigh"].ToInt32());
            }
            set
            {
                MemorySettings.Memory.Write(address.Add(MemorySettings.NetVars["m_iItemIDHigh"]), value);
            }
        }

        public BasePlayer GetOwner()
        {
            int index = BitConverter.ToInt32(readData, MemorySettings.NetVars["m_hOwner"].ToInt32());
            index &= 0xFFF;
            return SDKSettings.EntityList.GetPlayerByIndex(index);
        }

        public string GetWeaponName()
        {
            return GetItemDefinitionIndex().ToString();
        }

        public bool IsSpecial()
        {
            var index = GetItemDefinitionIndex();
            return index == ItemDefinitionIndex.C4 ||
                   index == ItemDefinitionIndex.ZEUS_X27 ||
                   index == ItemDefinitionIndex.DEFAULT_CT |
                   index == ItemDefinitionIndex.DEFAULT_T;
        }

        public bool IsKnife()
        {
            return GetWeaponName().ToLower().Contains("knife") || GetWeaponName().ToLower().Contains("default");
        }

        public bool IsGrenade()
        {
            return GetWeaponName().ToLower().Contains("grenade");
        }

        public int GetState()
        {
            return BitConverter.ToInt16(readData, MemorySettings.NetVars["m_iState"].ToInt32());
        }
    }
}