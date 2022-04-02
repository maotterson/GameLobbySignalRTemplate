using GameLobbySignalRTemplate.Shared.Models.Alias;
using GameLobbySignalRTemplate.Server.Models.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GameLobbySignalRTemplate.Server.Services
{
    public class MongoDBService
    {
        public IMongoDatabase MongoDatabase { get; private set; }
        private CollectionService _collectionService;

        public MongoDBService(
            IOptions<GameDatabaseSettings> gameDBSettings,
            CollectionService collectionService
            )
        {
            var mongoClient = new MongoClient(gameDBSettings.Value.ConnectionString);
            MongoDatabase = mongoClient.GetDatabase(gameDBSettings.Value.DatabaseName);
            _collectionService = collectionService;
        }

        public async Task<IList<Suffix>> GetSuffixesAsync()
        {
            var collection = _collectionService.CollectionsDictionary["Suffix"];
            var suffixCollection = MongoDatabase.GetCollection<Suffix>(collection);
            return await suffixCollection.Find(_ => true).ToListAsync();
        }

        public async Task<IList<Prefix>> GetPrefixesAsync()
        {
            var collection = _collectionService.CollectionsDictionary["Prefix"];
            var prefixCollection = MongoDatabase.GetCollection<Prefix>(collection);
            return await prefixCollection.Find(_ => true).ToListAsync();
        }
    }
}

