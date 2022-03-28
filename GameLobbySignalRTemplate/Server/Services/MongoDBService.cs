using GameLobbySignalRTemplate.Server.Models.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GameLobbySignalRTemplate.Server.Services
{
    public class MongoDBService
    {
        public IMongoDatabase _mongoDatabase { get; private set; }

        public MongoDBService(IOptions<GameDatabaseSettings> gameDBSettings)
        {
            var mongoClient = new MongoClient(gameDBSettings.Value.ConnectionString);
            _mongoDatabase = mongoClient.GetDatabase(gameDBSettings.Value.DatabaseName);
        }
    }
}
