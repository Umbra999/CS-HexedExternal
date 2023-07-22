using Hexed.Core;
using Hexed.Memory;
using Hexed.Wrappers;
using System.Numerics;
using System.Runtime.InteropServices;
using static Hexed.SDK.Objects.Structs;

namespace Hexed.SDK.Base
{
    internal class BaseEntity
    {
        protected byte[] readData;
        protected IntPtr address;

        public BaseEntity(IntPtr address)
        {
            this.address = address;
            Update();
        }

        public BaseEntity(BaseEntity ent)
        {
            readData = ent.GetData();
        }

        internal byte[] GetData()
        {
            return readData;
        }

        public void Update()
        {
            readData = GameManager.Memory.ReadByteArray(address, GameManager.NetVars.MaxValue() + Marshal.SizeOf(typeof(Vector3)));
        }

        public IntPtr Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }

        public int GetIndex()
        {
            return BitConverter.ToInt32(readData, GameManager.NetVars["m_dwIndex"].ToInt32());
        }

        public bool IsDormant()
        {
            return BitConverter.ToBoolean(readData, GameManager.NetVars["m_bDormant"].ToInt32());
        }

        public ClientClass GetClientClass()
        {
            var vt = new IntPtr(BitConverter.ToInt32(readData, 8));
            var fn = GameManager.Memory.Read<IntPtr>(vt + 8);
            var result = GameManager.Memory.Read<IntPtr>(fn + 1);
            return GameManager.Memory.Read<ClientClass>(result);
        }

        public int GetClientClassAddress()
        {
            var vt = new IntPtr(BitConverter.ToInt32(readData, 8));
            var fn = GameManager.Memory.Read<IntPtr>(vt + 8);
            var result = GameManager.Memory.Read<int>(fn + 1);
            return result;
        }

        public Vector3 GetPosition()
        {
            byte[] vecData = new byte[12];
            Buffer.BlockCopy(readData, GameManager.NetVars["m_vecOrigin"].ToInt32(), vecData, 0, 12);
            return ProcessMemory.BytesTo<Vector3>(vecData);
        }
    }
}
