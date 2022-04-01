using GameLobbySignalRTemplate.Server.Models.Database;

namespace GameLobbySignalRTemplate.Server.Models.Collections
{
    public class CollectionsSettings
    {
        public Dictionary<string, PropertySetting> Collections { get; set; } = null!;
    }
}
