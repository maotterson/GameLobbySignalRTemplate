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
        private IList<Prefix> prefixes;
        private IList<Suffix> suffixes;
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
        public async Task<string> GetRandomAliasAsync()
        {
            if (prefixes is null) prefixes = await GetPrefixesAsync();
            if (suffixes is null) suffixes = await GetSuffixesAsync();

            Random random = new Random();
            Prefix prefix = prefixes.ToList()[random.Next(prefixes.ToList().Count)];
            Suffix suffix = suffixes.ToList()[random.Next(suffixes.ToList().Count)];

            return $"{prefix.Name}{suffix.Name}";
        }
        private async Task<IList<Prefix>> GetPrefixesAsync()
        {
            var prefixes = await _redisService.GetPrefixesAsync()!;
            if (prefixes is null)
            {
                prefixes = await _mongoDBService.GetPrefixesAsync();
                await _redisService.CacheListAsync(prefixes);
            }
            
            return prefixes;
        }
        
        private async Task<IList<Suffix>> GetSuffixesAsync()
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
