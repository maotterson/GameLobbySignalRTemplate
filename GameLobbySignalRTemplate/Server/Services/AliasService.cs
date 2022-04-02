using GameLobbySignalRTemplate.Shared.Models.Alias;
using GameLobbySignalRTemplate.Server.Models.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GameLobbySignalRTemplate.Server.Services
{
    public class AliasService
    {
        private IList<Prefix> prefixes = null!;
        private IList<Suffix> suffixes = null!;
        private CollectionService _collectionService;
        private RedisCacheService _redisService;
        private MongoDBService _mongoDBService;

        public AliasService(
            MongoDBService mongoDBService,
            RedisCacheService redisCacheService,
            CollectionService collectionService,
            IOptions<GameDatabaseSettings> gameDBSettings)
        {
            _redisService = redisCacheService;
            _mongoDBService = mongoDBService;
            _collectionService = collectionService;
        }
        public async Task<Alias> GetRandomAliasAsync()
        {
            if (prefixes is null) prefixes = await PopulatePrefixesAsync();
            if (suffixes is null) suffixes = await PopulateSuffixesAsync();

            Random random = new Random();
            Prefix prefix = prefixes.ToList()[random.Next(prefixes.ToList().Count)];
            Suffix suffix = suffixes.ToList()[random.Next(suffixes.ToList().Count)];
            Alias alias = new(prefix, suffix);

            return alias;
        }
        private async Task<IList<Prefix>> PopulatePrefixesAsync()
        {
            var prefixes = await _redisService.GetPrefixesAsync()!;
            if (prefixes is null)
            {
                prefixes = await _mongoDBService.GetPrefixesAsync();
                await _redisService.CacheListAsync(prefixes);
            }
            
            return prefixes;
        }
        
        private async Task<IList<Suffix>> PopulateSuffixesAsync()
        {
            var suffixes = await _redisService.GetSuffixesAsync()!;
            if (suffixes is null)
            {
                suffixes = await _mongoDBService.GetSuffixesAsync();
                await _redisService.CacheListAsync(suffixes);
            }

            return suffixes;
        }
    }
}
