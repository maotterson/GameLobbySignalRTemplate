using GameLobbySignalRTemplate.Shared.Models.Alias;
using GameLobbySignalRTemplate.Server.Models.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using GameLobbySignalRTemplate.Shared.Models.Alias.Utils;

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
        public async Task AddUsedAlias(AliasEntity aliasEntity)
        {
            var collectionName = _collectionService.CollectionsDictionary["UsedAliases"];
            var aliasCollection = MongoDatabase.GetCollection<AliasEntity>(collectionName);
            await aliasCollection.InsertOneAsync(aliasEntity);
        }
        public async Task<IList<Suffix>> GetSuffixesAsync()
        {
            var collectionName = _collectionService.CollectionsDictionary["Suffix"];
            var suffixCollection = MongoDatabase.GetCollection<Suffix>(collectionName);
            return await suffixCollection.Find(_ => true).ToListAsync();
        }

        public async Task<IList<Prefix>> GetPrefixesAsync()
        {
            var collectionName = _collectionService.CollectionsDictionary["Prefix"];
            var prefixCollection = MongoDatabase.GetCollection<Prefix>(collectionName);
            return await prefixCollection.Find(_ => true).ToListAsync();
        }

        public async Task<IList<AliasEntity>> GetUsedAliasesAsync()
        {

            var collectionName = _collectionService.CollectionsDictionary["UsedAliases"];
            var usedAliasCollection = MongoDatabase.GetCollection<AliasEntity>(collectionName);
            return await usedAliasCollection.Find(_ => true).ToListAsync();
        }
    }
}

