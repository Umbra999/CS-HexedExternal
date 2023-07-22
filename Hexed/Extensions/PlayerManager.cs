using Hexed.SDK.Base;
using Hexed.SDK;
using Hexed.Core;
using Hexed.Memory.Manager;

namespace Hexed.Extensions
{
    internal class PlayerManager
    {
        private static IntPtr entityAddress = IntPtr.Zero;

        public static BasePlayer GetPlayerByIndex(int index)
        {
            List<BasePlayer> Players = GetPlayers();

            return Players == null ? null : Players.FirstOrDefault(player => player.GetIndex() == index);
        }

        public static BaseEntity GetEntityByIndex(int index)
        {
            List<BaseEntity> Entities = GetEntities();

            return Entities == null ? null : Entities.FirstOrDefault(ent => ent.GetIndex() == index);
        }

        public static BasePlayer GetLocalPlayer()
        {
            List<BasePlayer> Players = GetPlayers();

            return Players?.FirstOrDefault(player => player.GetIndex() == (EngineClient.LocalPlayerIndex + 1));
        }

        public static List<BasePlayer> GetPlayers()
        {
            if (entityAddress == IntPtr.Zero) entityAddress = SignatureManager.GetEntityList();

            List<BasePlayer> players = new();
            byte[] entityList = GameManager.Memory.ReadByteArray(entityAddress, 64 * 0x10);
            for (int i = 1; i < 64/*BaseClient.GlobalVars.maxClients*/; i++)
            {
                IntPtr entity = BitConverter.ToInt32(entityList, i * 0x10);

                if (entity == IntPtr.Zero) continue;

                BasePlayer player = new(entity);
                players.Add(player);
            }

            return players;
        }

        public static List<BaseEntity> GetEntities()
        {
            if (entityAddress == IntPtr.Zero) entityAddress = SignatureManager.GetEntityList();

            List<BaseEntity> entities = new();
            byte[] entityList = GameManager.Memory.ReadByteArray(entityAddress + 64 * 0x10, 4096 * 0x10);

            for (int i = 64; i < 4096; i++)
            {
                IntPtr entity = BitConverter.ToInt32(entityList, i * 0x10);

                if (entity == IntPtr.Zero) continue;

                BaseEntity ent = new(entity);
                entities.Add(ent);
            }

            return entities;
        }
    }
}
