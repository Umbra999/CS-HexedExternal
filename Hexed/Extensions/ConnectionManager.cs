using Hexed.SDK;
using Hexed.SDK.Base;

namespace Hexed.Extensions
{
    internal class ConnectionManager
    {
        public static bool IsInWorld()
        {
            List<BaseEntity> Entities = PlayerManager.GetEntities();

            return EngineClient.IsInGame && Entities != null && Entities.Count > 0;
        }
    }
}
