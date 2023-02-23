using Hexed.Memory;

namespace Hexed.SDK.Base
{
    internal class BaseViewmodel : BaseEntity
    {
        public BaseViewmodel(IntPtr address) : base(address) { }

        public int GetModelIndex()
        {
            return BitConverter.ToInt32(readData, MemorySettings.NetVars["m_nModelIndex"].ToInt32());
        }

        public int GetWeapon()
        {
            return BitConverter.ToInt32(readData, MemorySettings.NetVars["m_hWeapon"].ToInt32()) & 0xFFF;
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
                stub = MemorySettings.Memory.Allocate(opcodes.Length);
                MemorySettings.Memory.WriteByteArray(stub.Address, opcodes);
                namePtr = MemorySettings.Memory.Allocate(modelName.Length * 2 + 1);
                MemorySettings.Memory.Write(stub.Address + 11, namePtr.Address); //thisptr

                written = true;
            }

            var virtualMethod = MemorySettings.Memory.GetVirtualFunction(address, 242);

            MemorySettings.Memory.WriteString(namePtr.Address, modelName, System.Text.Encoding.UTF8);

            MemorySettings.Memory.Write(stub.Address + 1, address);
            MemorySettings.Memory.Write(stub.Address + 6, weapon);
            MemorySettings.Memory.Write(stub.Address + 16, virtualMethod);

            MemorySettings.Memory.Execute(stub.Address);
        }
    }
}
