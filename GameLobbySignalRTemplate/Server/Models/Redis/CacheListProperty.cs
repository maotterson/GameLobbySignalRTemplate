using GameLobbySignalRTemplate.Server.Utils;
using StackExchange.Redis;

namespace GameLobbySignalRTemplate.Server.Models.Redis
{
    public class CacheListProperty<T> : ICacheProperty<IList<T>>
    {
        public string Key { get; set; } = null!;
        public IList<T> Value { get; set; } = null!;

        public async Task<bool> TryCacheAsync(IDatabase cache)
        {
            long size = await cache.ListLengthAsync(Key);
            if (size == 0) return false;

            Value = new List<T>();
            var redisArray = await cache.ListRangeAsync(Key);
            var json = redisArray.ToStringArray();
            foreach (var item in json)
            {
                Value.Add(item.DeserializeJson<T>());
            }
            return true;
        }
    }
}
