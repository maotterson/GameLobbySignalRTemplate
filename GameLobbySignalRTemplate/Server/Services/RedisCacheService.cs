using GameLobbySignalRTemplate.Server.Models;
using GameLobbySignalRTemplate.Server.Models.Redis;
using GameLobbySignalRTemplate.Server.Utils;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace GameLobbySignalRTemplate.Server.Services
{
    public class RedisCacheService
    {
        public ConnectionMultiplexer Redis { get; private set; }
        private CollectionService _collectionService;

        public RedisCacheService(
            IOptions<RedisCacheSettings> redisSettings,
            CollectionService collectionService
        )
        {
            Redis = ConnectionMultiplexer.Connect(redisSettings.Value.Configuration);
            _collectionService = collectionService;
        }

        public async Task<IList<Suffix>>? GetSuffixesAsync()
        {
            var collection = _collectionService.CollectionsDictionary["Suffix"];
            var cache = Redis.GetDatabase();
            var list = new List<Suffix>();
            if (await cache.ListLengthAsync(collection) == 0) return null!;

            var redisArray = await cache.ListRangeAsync(collection);
            var json = redisArray.ToStringArray();
            foreach (var item in json)
            {
                list.Add(item.DeserializeJson<Suffix>());
            }
            return list;
        }

        public async Task<IList<Prefix>>? GetPrefixesAsync()
        {
            var collection = _collectionService.CollectionsDictionary["Prefix"];
            var cache = Redis.GetDatabase();
            var list = new List<Prefix>();
            if (await cache.ListLengthAsync(collection) == 0) return null!;

            var redisArray = await cache.ListRangeAsync(collection);
            var json = redisArray.ToStringArray();
            foreach (var item in json)
            {
                list.Add(item.DeserializeJson<Prefix>());
            }
            return list;
        }

        public async Task CacheListAsync<T>(IList<T> list)
        {
            var property = typeof(T).Name;
            var collection = _collectionService.CollectionsDictionary[property];
            foreach (var item in list)
            {
                Redis.GetDatabase().ListLeftPush(collection, item.SerializeJson());
            }
        }

    }
}