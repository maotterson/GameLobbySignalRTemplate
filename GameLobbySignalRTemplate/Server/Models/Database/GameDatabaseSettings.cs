namespace GameLobbySignalRTemplate.Server.Models.Database
{
    public class GameDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string PrefixCollectionName { get; set; } = null!;
        public string SuffixCollectionName { get; set; } = null!;
    }
}
