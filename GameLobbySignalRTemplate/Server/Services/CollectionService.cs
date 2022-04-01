using GameLobbySignalRTemplate.Server.Models.Collections;
using GameLobbySignalRTemplate.Server.Models.Database;
using Microsoft.Extensions.Options;

namespace GameLobbySignalRTemplate.Server.Services
{
    public class CollectionService
    {
        public Dictionary<string, string> CollectionsDictionary { get; private set; }
        public CollectionService(IOptions<CollectionsSettings> collectionsSettings)
        {
            CollectionsDictionary = collectionsSettings.Value.CollectionPairs;
        }
    }
}
