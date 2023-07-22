using Hexed.Core;
using Hexed.Memory;

namespace Hexed.SDK.Base
{
    internal class BaseViewmodel : BaseEntity
    {
        public BaseViewmodel(IntPtr address) : base(address) { }

        public int GetModelIndex()
        {
            return BitConverter.ToInt32(readData, GameManager.NetVars["m_nModelIndex"].ToInt32());
        }

        public int GetWeapon()
        {
            return BitConverter.ToInt32(readData, GameManager.NetVars["m_hWeapon"].ToInt32()) & 0xFFF;
        }

        public void GetModelIndex(string modelName)
        {
            byte[] opcodes =
                {
                    0xB9, 0x00, 0x00, 0x00, 0x00,   // mov ecx, int
                    0x68, 0x00, 0x00, 0x00, 0x00,   // push IntPtr
                    0xB8, 0x00, 0x00, 0x00, 0x00,   // mov eax, int
                    0xFF, 0xD0,                     // call eax
                    0xA3, 0x00, 0x00, 0x00, 0x00,   // mov IntPtr, eax
                    0xC3                            // ret
                };
        }

        private static bool written = false;
        private static RemoteAllocation stub;
        private static RemoteAllocation namePtr;
        public void SetWeaponModel(string modelName, IntPtr weapon)
        {
            if (!written)
            {
                byte[] opcodes =
                {
                    0xB9, 0x00, 0x00, 0x00, 0x00,   // mov ecx, int
                    0x68, 0x00, 0x00, 0x00, 0x00,   // push IntPtr
                    0x68, 0x00, 0x00, 0x00, 0x00,   // push IntPtr
                    0xB8, 0x00, 0x00, 0x00, 0x00,   // mov eax, int
                    0xFF, 0xD0,                     // call eax
                    0xC3                            // ret
                };
                stub = GameManager.Memory.Allocate(opcodes.Length);
                GameManager.Memory.WriteByteArray(stub.Address, opcodes);
                namePtr = GameManager.Memory.Allocate(modelName.Length * 2 + 1);
                GameManager.Memory.Write(stub.Address + 11, namePtr.Address); //thisptr

                written = true;
            }

            var virtualMethod = GameManager.Memory.GetVirtualFunction(address, 242);

            GameManager.Memory.WriteString(namePtr.Address, modelName, System.Text.Encoding.UTF8);

            GameManager.Memory.Write(stub.Address + 1, address);
            GameManager.Memory.Write(stub.Address + 6, weapon);
            GameManager.Memory.Write(stub.Address + 16, virtualMethod);

            GameManager.Memory.Execute(stub.Address);
        }
    }
}
