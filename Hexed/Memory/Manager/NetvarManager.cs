using Hexed.Core;

namespace Hexed.Memory.Manager
{
    internal static class NetvarManager
    {
        private static IntPtr _clientClassesHead;
        private static IntPtr ClientClassesHead
        {
            get
            {
                if (_clientClassesHead == IntPtr.Zero) _clientClassesHead = SignatureManager.GetClientClassesHead();
                return _clientClassesHead;
            }
        }

        private static IntPtr SearchInSubSubTable(IntPtr subTable, string searchFor)
        {
            IntPtr current = MemoryHandler.Memory.Read<IntPtr>(MemoryHandler.Memory.Read<IntPtr>(subTable + 0x28));
            while (true)
            {
                string entryName = MemoryHandler.Memory.ReadString(MemoryHandler.Memory.Read<IntPtr>(current));

                if (entryName == "") break;

                if (entryName.Length < 1) break;

                if (entryName.Length > 3)
                {
                    IntPtr offset = MemoryHandler.Memory.Read<IntPtr>(current + 0x2C);
                    if (entryName.Equals(searchFor)) return offset;
                }

                IntPtr subSubTable = MemoryHandler.Memory.Read<IntPtr>(current + 0x28);

                if (subSubTable != IntPtr.Zero)
                {
                    IntPtr a = SearchInSubSubTable(current, searchFor);
                    if (a != IntPtr.Zero) return a;
                }
                current += 0x3C;
            }

            return IntPtr.Zero;

        }

        private static IntPtr SearchInSubtable(IntPtr subTable, string searchFor)
        {
            IntPtr current = subTable;
            while (true)
            {
                string entryName = MemoryHandler.Memory.ReadString(MemoryHandler.Memory.Read<IntPtr>(current));

                if (entryName == "") break;

                if (entryName.Length < 1) break;

                if (entryName == "baseclass")
                {
                    IntPtr a = SearchInBaseClass(current, searchFor);
                    if (a != IntPtr.Zero) return a;
                }

                if (entryName == "cslocaldata")
                {
                    IntPtr a = SearchInCSLocalData(current, searchFor);
                    if (a != IntPtr.Zero) return a;
                }

                if (entryName == "localdata")
                {
                    IntPtr a = SearchInLocalData(current, searchFor);
                    if (a != IntPtr.Zero) return a;
                }

                int subSubTable = MemoryHandler.Memory.Read<int>(current + 0x28);

                if (subSubTable > 0)
                {
                    IntPtr a = SearchInSubSubTable(current, searchFor);
                    if (a != IntPtr.Zero) return a;
                }

                IntPtr offset = MemoryHandler.Memory.Read<IntPtr>(current + 0x2C);
                if (entryName == searchFor) return offset;

                current += 0x3C;
            }

            return IntPtr.Zero;
        }

        private static IntPtr SearchInBaseClass(IntPtr baseClass, string searchFor)
        {
            IntPtr a = SearchInSubtable(baseClass + 0x3C, searchFor);
            if (a != IntPtr.Zero) return a;

            string className = MemoryHandler.Memory.ReadString(MemoryHandler.Memory.Read<IntPtr>(baseClass));

            if (className == "baseclass") return SearchInBaseClass(MemoryHandler.Memory.Read<IntPtr>(MemoryHandler.Memory.Read<IntPtr>(baseClass + 0x28)), searchFor);

            return IntPtr.Zero;
        }

        private static IntPtr SearchInCSLocalData(IntPtr csLocalData, string searchFor)
        {
            IntPtr a = SearchInSubtable(csLocalData + 0x28, searchFor);
            if (a != IntPtr.Zero) return a;

            string className = MemoryHandler.Memory.ReadString(MemoryHandler.Memory.Read<IntPtr>(csLocalData));

            if (className == "cslocaldata") return SearchInBaseClass(MemoryHandler.Memory.Read<IntPtr>(MemoryHandler.Memory.Read<IntPtr>(csLocalData + 0x28)), searchFor);

            return IntPtr.Zero;
        }

        private static IntPtr SearchInLocalData(IntPtr localData, string searchFor)
        {
            IntPtr a = SearchInSubtable(localData + 0x28, searchFor);

            if (a != IntPtr.Zero) return a;

            string className = MemoryHandler.Memory.ReadString(MemoryHandler.Memory.Read<IntPtr>(localData));

            if (className == "localdata") return SearchInBaseClass(MemoryHandler.Memory.Read<IntPtr>(MemoryHandler.Memory.Read<IntPtr>(localData + 0x28)), searchFor);

            return IntPtr.Zero;
        }

        private static IntPtr SearchInTableFor(IntPtr table, string searchFor)
        {
            IntPtr current = MemoryHandler.Memory.Read<IntPtr>(MemoryHandler.Memory.Read<IntPtr>(table + 0xC));
            while (true)
            {
                if (MemoryHandler.Memory.Read<IntPtr>(current) == IntPtr.Zero) break;

                string entryName = MemoryHandler.Memory.ReadString(MemoryHandler.Memory.Read<IntPtr>(current));

                if (entryName.Length < 1) break;

                if (entryName == "baseclass") return SearchInBaseClass(current, searchFor);

                if (entryName == "cslocaldata") return SearchInCSLocalData(current, searchFor);

                if (entryName == "localdata") return SearchInLocalData(current, searchFor);

                IntPtr offset = MemoryHandler.Memory.Read<IntPtr>(current + 0x2C);
                if (entryName.Equals(searchFor)) return offset;

                current += 0x3C;

            }

            return IntPtr.Zero;
        }

        private static IntPtr GetTable(string wantedTable)
        {
            IntPtr current = ClientClassesHead;

            while (true)
            {
                string className = MemoryHandler.Memory.ReadString(MemoryHandler.Memory.Read<IntPtr>(current + 0x8));
                string tableName = MemoryHandler.Memory.ReadString(MemoryHandler.Memory.Read<IntPtr>(MemoryHandler.Memory.Read<IntPtr>(current + 0xC) + 0xC));

                if (className.Equals(wantedTable) || tableName.Equals(wantedTable)) return current;

                current = MemoryHandler.Memory.Read<IntPtr>(current + 0x10);
                if (current == IntPtr.Zero) break;
            }

            return IntPtr.Zero;
        }

        public static IntPtr GetOffset(string table, string entry)
        {
            IntPtr tableAddress = GetTable(table);
            IntPtr offset = SearchInTableFor(tableAddress, entry);
            return offset;
        }
    }
}
