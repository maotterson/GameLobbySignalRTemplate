using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbySignalRTemplate.Shared.Models.Alias
{
    public record Alias
    {
        private Prefix prefix;
        private Suffix suffix;

        public Alias(Prefix prefix, Suffix suffix)
        {
            this.prefix = prefix;
            this.suffix = suffix;
        }

        public string Value => prefix.Name + " " + suffix.Name;
    }
}
