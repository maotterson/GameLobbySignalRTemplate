namespace GameLobbySignalRTemplate.Server.Services
{
    public class AliasService
    {
        private IEnumerable<string> prefixes;
        private IEnumerable<string> suffixes;

        private void GetPrefixes()
        {

        }

        private void GetSuffixes()
        {

        }

        public string GetAlias()
        {
            if(prefixes is null)
            {
                GetPrefixes();
            }
            if(suffixes is null)
            {
                GetSuffixes();
            }

            Random random = new Random();
            string prefix = prefixes.ToList()[random.Next(prefixes.ToList().Count)];
            string suffix = suffixes.ToList()[random.Next(suffixes.ToList().Count)];

            return $"{prefix}{suffix}";
        }
    }
}
