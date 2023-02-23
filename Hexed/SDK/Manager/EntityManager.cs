using Hexed.Memory;
using Hexed.Memory.Manager;
using Hexed.SDK.Base;
using Hexed.Wrappers;

namespace Hexed.SDK.Manager
{
    internal class EntityManager
    {
        private static IntPtr entityAddress;

        public static void UpdatePlayers()
        {
            if (SDKSettings.EntityList == null) SDKSettings.EntityList = new EntityList();
            if (entityAddress == IntPtr.Zero) entityAddress = SignatureManager.GetEntityList();

            List<BasePlayer> players = new();
            byte[] entityList = MemorySettings.Memory.ReadByteArray(entityAddress, 64 * 0x10);
            for (int i = 1; i < 64/*BaseClient.GlobalVars.maxClients*/; i++)
            {
                IntPtr entity = BitConverter.ToInt32(entityList, i * 0x10);

                if (entity == IntPtr.Zero) continue;

                BasePlayer player = new(entity);
                players.Add(player);
            }

            //Logger.LogDebug(players.Count);

            SDKSettings.EntityList.Players = players;
        }

        public static void UpdateEntities()
        {
            if (SDKSettings.EntityList == null) SDKSettings.EntityList = new EntityList();
            if (entityAddress == IntPtr.Zero) entityAddress = SignatureManager.GetEntityList();

            List<BaseEntity> entities = new();
            byte[] entityList = MemorySettings.Memory.ReadByteArray(entityAddress + 64 * 0x10, 4096 * 0x10);

            for (int i = 64; i < 4096; i++)
            {
                IntPtr entity = BitConverter.ToInt32(entityList, i * 0x10);

                if (entity == IntPtr.Zero) continue;

                BaseEntity ent = new(entity);
                entities.Add(ent);
            }

            SDKSettings.EntityList.Entities = entities;
        }
    }
}
