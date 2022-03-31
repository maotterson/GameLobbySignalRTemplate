using GameLobbySignalRTemplate.Server.Models;
using GameLobbySignalRTemplate.Server.Models.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StackExchange.Redis;
using GameLobbySignalRTemplate.Server.Utils;

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
            //check cache 
            var cache = _redis.GetDatabase();
            long numPrefixes = await cache.ListLengthAsync(_prefixCollectionName);

            IList<Prefix> prefixes;
            if(numPrefixes is 0)
            {
                var prefixCollection = _mongoDB.GetCollection<Prefix>(_prefixCollectionName);
                prefixes = await prefixCollection.Find(_ => true).ToListAsync();
                foreach(var prefix in prefixes)
                {
                    cache.ListLeftPush(_prefixCollectionName, prefix.SerializeJson());
                }
            }
            else
            {
                prefixes = new List<Prefix>();
                var redisArray = await cache.ListRangeAsync(_prefixCollectionName);
                var json = redisArray.ToStringArray();
                foreach(var item in json)
                {
                    prefixes.Add(item.DeserializeJson<Prefix>());
                }
            }
            return prefixes;
        }

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
