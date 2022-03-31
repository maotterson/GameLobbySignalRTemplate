using StackExchange.Redis;

namespace GameLobbySignalRTemplate.Server.Models.Redis
{
    public interface ICacheProperty<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }
        public Task<bool> TryCacheAsync(IDatabase cache);
    }
}
