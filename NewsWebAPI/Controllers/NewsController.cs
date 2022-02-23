using Microsoft.AspNetCore.Mvc;
using NewsWebAPI.Models;
using NewsWebAPI.Services;

namespace NewsWebAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class NewsController : ControllerBase
    {

        private readonly ILogger<NewsController> _logger;
        private readonly NewsService _newsService;

        public NewsController(ILogger<NewsController> logger,
            NewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        [HttpGet("best20")]
        public async Task<IEnumerable<NewsArticleDTO>> GetBest20Async()
        {
            _logger.LogDebug("NewsController GetBest20");
            return await _newsService.fetchBestAsync(20);
        }
    }
}