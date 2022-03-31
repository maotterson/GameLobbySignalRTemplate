using GameLobbySignalRTemplate.Server.Models.Redis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace GameLobbySignalRTemplate.Server.Services
{
    public class RedisCacheService
    {
        public ConnectionMultiplexer Redis { get; private set; }

        public RedisCacheService(IOptions<RedisCacheSettings> redisSettings)
        {
            Redis = ConnectionMultiplexer.Connect(redisSettings.Value.Configuration);
        }
    }
}
