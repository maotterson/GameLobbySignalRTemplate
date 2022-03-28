using GameLobbySignalRTemplate.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameLobbySignalRTemplate.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliasController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly AliasService _aliasService;

        public AliasController(ILogger<WeatherForecastController> logger, AliasService aliasService)
        {
            _logger = logger;
            _aliasService = aliasService;
        }

        [HttpGet]
        public string Get()
        {
            return _aliasService.GetAlias();
        }
    }
}