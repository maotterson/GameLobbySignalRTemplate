using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbySignalRTemplate.Shared.Models.Alias.Utils
{
    public static class AliasEntityHelpers
    {
        public static AliasEntity AsAliasEntity(this Alias alias)
        {
            AliasEntity aliasEntity = new()
            {
                Value = alias.Value
            };

            return aliasEntity;
        }
    }
}
