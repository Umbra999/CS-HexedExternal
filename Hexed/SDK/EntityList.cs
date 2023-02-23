using Hexed.SDK.Base;

namespace Hexed.SDK
{
    internal class EntityList
    {
        public List<BasePlayer> Players;
        public List<BaseEntity> Entities;

        public BasePlayer GetPlayerByIndex(int index)
        {
            return Players == null ? null : Players.FirstOrDefault(player => player.GetIndex() == index);
        }

        public BaseEntity GetEntityByIndex(int index)
        {
            return Entities == null ? null : Entities.FirstOrDefault(ent => ent.GetIndex() == index);
        }

        public BasePlayer GetLocalPlayer()
        {
            return Players == null ? null : Players.FirstOrDefault(player => player.GetIndex() == (EngineClient.LocalPlayerIndex + 1));
        }
    }
}
