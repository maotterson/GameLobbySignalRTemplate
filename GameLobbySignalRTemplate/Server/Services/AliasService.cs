namespace GameLobbySignalRTemplate.Server.Services
{
    public class AliasService
    {
        private readonly IEnumerable<string> prefixes = new String[]{ "Swashbuckler"};
        private readonly IEnumerable<string> suffixes = new String[] { "Steve" };
    }
}
