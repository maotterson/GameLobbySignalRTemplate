using GameLobbySignalRTemplate.Server.Models;
using GameLobbySignalRTemplate.Server.Models.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StackExchange.Redis;
using GameLobbySignalRTemplate.Server.Utils;
using GameLobbySignalRTemplate.Server.Models.Redis;

namespace GameLobbySignalRTemplate.Server.Services
{
    public class AliasService
    {
        private IEnumerable<Prefix> prefixes;
        private IEnumerable<Suffix> suffixes;
        private IMongoDatabase _mongoDB;
        private ConnectionMultiplexer _redis;
        private string _prefixCollectionName;
        private string _suffixCollectionName;

        public AliasService(
            MongoDBService mongoDBService,
            RedisCacheService redisCacheService,
            IOptions<GameDatabaseSettings> gameDBSettings)
        {
            _redis = redisCacheService.Redis;
            _mongoDB = mongoDBService.MongoDatabase;
            _prefixCollectionName = gameDBSettings.Value.PrefixCollectionName;
            _suffixCollectionName = gameDBSettings.Value.SuffixCollectionName;
        }
        private async Task<IEnumerable<Prefix>> GetPrefixesAsync()
        {
            var cache = _redis.GetDatabase();
            CacheListProperty<Prefix> cachedPrefixes = new()
            {
                Key = _prefixCollectionName
            };
            bool isCached = await cachedPrefixes.TryCacheAsync(cache);
            if (isCached) return cachedPrefixes.Value;
            
            var prefixCollection = _mongoDB.GetCollection<Prefix>(_prefixCollectionName);
            var prefixes = await prefixCollection.Find(_ => true).ToListAsync();
            foreach(var prefix in prefixes)
            {
                cache.ListLeftPush(_prefixCollectionName, prefix.SerializeJson());
            }
            return prefixes;
        }
        /*
        private async Task<IEnumerable<T>> GetAndCacheAsync()
        {
            var cache = _redis.GetDatabase();
            ICacheProperty<T> cachedProperty = new()
            {
                Key = dbCollectionName
            }
            bool isCached = await cachedProperty.TryCacheAsync(cache);
            if(isCached) return cachedProperty.Value;
            
            var collection = _mongoDB.GetCollection<T>(dbCollectionName);
            var uncachedProperty = await collection.Find(_ => true).ToListAsync();
            foreach(var item in uncachedProperty)
            {
                cache.ListLeftPush(dbCollectionName, item.SerializeJson());
            }
            return uncachedProperty;
        }
        */
        private async Task<IEnumerable<Suffix>> GetSuffixesAsync()
        {
            var suffixCollection = _mongoDB.GetCollection<Suffix>(_suffixCollectionName);
            return await suffixCollection.Find(_ => true).ToListAsync();
        }

        public async Task<string> GetRandomAliasAsync()
        {
            if(prefixes is null)
            {
                prefixes = await GetPrefixesAsync();
            }
            if(suffixes is null)
            {
                suffixes = await GetSuffixesAsync();
            }

            Random random = new Random();
            Prefix prefix = prefixes.ToList()[random.Next(prefixes.ToList().Count)];
            Suffix suffix = suffixes.ToList()[random.Next(suffixes.ToList().Count)];

            return $"{prefix.Name}{suffix.Name}";
        }
    }
}
