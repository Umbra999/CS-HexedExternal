using System.Runtime.Serialization;

namespace Hexed.HexedServer
{
    internal class ServerObjects
    {
        public enum KeyPermissionType
        {
            VRChat = 0,
            SeaOfThieves = 1,
            CSGO = 2,
            Minecraft = 3,
            VRChatBot = 4,
            VRChatReuploader = 5
        }

        [DataContract]
        public class UserData
        {
            [DataMember]
            public string Username { get; set; }
            [DataMember]
            public string Token { get; set; }
            [DataMember]
            public KeyPermissionType[] KeyAccess { get; set; }
        }
    }
}
