using Microsoft.AspNetCore.Mvc;
using NewsWebAPI.Models;
using NewsWebAPI.Services;

namespace NewsWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet(Name = "best20")]
        public IEnumerable<NewsArticle> Get()
        {
            return _newsService.fetchBest(20);
        }
    }
}