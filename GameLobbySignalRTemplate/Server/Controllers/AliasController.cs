using GameLobbySignalRTemplate.Server.Services;
using GameLobbySignalRTemplate.Shared.Models.Alias;
using GameLobbySignalRTemplate.Shared.Models.Alias.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GameLobbySignalRTemplate.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliasController : ControllerBase
    {
        private readonly ILogger<AliasController> _logger;
        private readonly AliasService _aliasService;

        public AliasController(
            ILogger<AliasController> logger,
            AliasService aliasService)
        {
            _logger = logger;
            _aliasService = aliasService;
        }

        [HttpGet]
        public async Task<ActionResult<AliasDto>> GetAsync()
        {
            var alias = await _aliasService.GetRandomAliasAsync();
            var aliasDto = alias.AsDto();
            return Ok(aliasDto);
        }
    }
}