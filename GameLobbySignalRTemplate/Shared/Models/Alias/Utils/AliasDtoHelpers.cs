using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLobbySignalRTemplate.Shared.Models.Alias.Utils
{
    public static class AliasDtoHelpers
    {
        public static AliasDto AsDto(this Alias alias)
        {
            AliasDto aliasDto = new()
            {
                Name = alias.Value
            };

            return aliasDto;
        }
    }
}
