using GameLobbySignalRTemplate.Shared.Models.Alias;
using GameLobbySignalRTemplate.Server.Models.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using GameLobbySignalRTemplate.Shared.Models.Alias.Utils;

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
            var alias = await GenerateAvailableAlias();

            return alias;
        }

        private async Task<Alias> GenerateAvailableAlias()
        {
            Alias alias = null!;
            bool? isAliasAvailable = false;
            await PopulateUsedAliasesCacheAsync(); // cache not stored locally due to frequent changes

            while (isAliasAvailable is false)
            {
                Random random = new Random();
                Prefix prefix = prefixes.ToList()[random.Next(prefixes.ToList().Count)];
                Suffix suffix = suffixes.ToList()[random.Next(suffixes.ToList().Count)];
                alias = new(prefix, suffix);

                isAliasAvailable = await _redisService.IsAliasAvailable(alias);
            }
            var aliasEntity = alias.AsAliasEntity();

            await _mongoDBService.AddUsedAlias(aliasEntity);
            await _redisService.CacheListItemAsync(aliasEntity, "UsedAliases");
            return alias;
        }

        private async Task PopulateUsedAliasesCacheAsync()
        {
            var isCached = await _redisService.IsCachedList("TakenAliases");
            if (isCached) return;

            var takenAliases = await _mongoDBService.GetUsedAliasesAsync();
            await _redisService.CacheListAsync(takenAliases, "TakenAliases");
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
