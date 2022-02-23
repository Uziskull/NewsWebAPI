using NewsWebAPI.Data;
using NewsWebAPI.Models;
using Newtonsoft.Json;

namespace NewsWebAPI.Services
{
    public class NewsService
    {
        private readonly ILogger<NewsService> _logger;
        private readonly NewsWebAPIContext _context;
        private readonly IConfiguration _config;

        public NewsService (ILogger<NewsService> logger, NewsWebAPIContext context, IConfiguration config)
        {
            this._logger = logger;
            this._context = context;
            this._config = config;
        }

        public async Task<IEnumerable<NewsArticleDTO>> fetchBestAsync(int num)
        {
            IEnumerable<int> bestStoriesAPI = await getBestStoriesAPI(num);
            IEnumerable<NewsArticleDAO> storiesDB = _context.NewsArticle.Where(news => bestStoriesAPI.Contains(news.Id));
            List<NewsArticleDAO> storiesToAdd = new List<NewsArticleDAO>();
            foreach (int storyId in bestStoriesAPI)
            {
                if (storiesDB.Where(news => news.Id.Equals(storyId)).Count() == 0)
                {
                    NewsArticleAPI? storyDetails = await getStoryDetailsAPI(storyId);
                    if (storyDetails == null)
                    {
                        _logger.LogDebug("Couldn't get story for id " + storyId);
                    }
                    else
                    {
                        NewsArticleDAO storyDAO = new NewsArticleDAO
                        {
                            Id = storyDetails.Id,
                            Title = storyDetails.Title,
                            PostedBy = storyDetails.By,
                            Uri = storyDetails.Url,
                            PostTime = DateTimeOffset.FromUnixTimeSeconds(storyDetails.Time).DateTime,
                            Score = storyDetails.Score,
                            CommentCount = storyDetails.Kids.Length,
                            Timestamp = DateTime.UtcNow
                        };
                        storiesToAdd.Add(storyDAO);
                    }
                }
            }
            if (storiesToAdd.Count > 0)
            {
                _context.NewsArticle.AddRange(storiesToAdd);

                int storyCount = _context.NewsArticle.Count();
                if (storyCount > _config.GetValue<int>("NewsAPI:Database:StoreLimit"))
                {
                    IEnumerable<NewsArticleDAO> storiesToRemove = _context.NewsArticle.TakeLast(storyCount - _config.GetValue<int>("NewsAPI:Database:StoreMinimum"));
                    _context.NewsArticle.RemoveRange(storiesToRemove);
                }

                await _context.SaveChangesAsync();
            }

            return _context.NewsArticle.Where(news => bestStoriesAPI.Contains(news.Id))
                .Select(s => new NewsArticleDTO
                {
                    Title = s.Title,
                    PostedBy = s.PostedBy,
                    Uri = s.Uri,
                    Time = s.PostTime,
                    Score = s.Score,
                    CommentCount = s.CommentCount
                });

        }

        private async Task<IEnumerable<int>> getBestStoriesAPI(int num)
        {
            _logger.LogDebug("NewsService getBestStoriesAPI");
            IEnumerable<int> bestStories = new List<int>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync(_config.GetValue<string>("NewsAPI:Endpoints:BestStories")))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        IEnumerable<int>? apiResponseList = JsonConvert.DeserializeObject<IEnumerable<int>>(apiResponse);
                        if (apiResponseList != null)
                        {
                            bestStories = apiResponseList.Take(num);
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogWarning("Unable to get best stories:");
                    _logger.LogWarning(e.StackTrace);
                }
            }
            return bestStories;
        }

        private async Task<NewsArticleAPI?> getStoryDetailsAPI(int id)
        {
            _logger.LogDebug("NewsService getStoryDetailsAPI");
            NewsArticleAPI? storyDetails = null;
            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync(
                        String.Format(_config.GetValue<string>("NewsAPI:Endpoints:StoryDetail"), id)))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        storyDetails = JsonConvert.DeserializeObject<NewsArticleAPI>(apiResponse);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogWarning("Unable to get story details:");
                    _logger.LogWarning(e.StackTrace);
                }
            }
            return storyDetails;
        }
    }
}
