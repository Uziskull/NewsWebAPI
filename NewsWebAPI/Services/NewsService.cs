using NewsWebAPI.Data;
using NewsWebAPI.Models;

namespace NewsWebAPI.Services
{
    public class NewsService
    {
        private readonly NewsWebAPIContext _context;

        public NewsService (NewsWebAPIContext context)
        {
            this._context = context;
        }

        public IEnumerable<NewsArticle> fetchBest(int num)
        {
            return null;
        }
    }
}
