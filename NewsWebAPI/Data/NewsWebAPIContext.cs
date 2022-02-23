#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Models;

namespace NewsWebAPI.Data
{
    public class NewsWebAPIContext : DbContext
    {
        public NewsWebAPIContext (DbContextOptions<NewsWebAPIContext> options)
            : base(options)
        {
        }

        public DbSet<NewsWebAPI.Models.NewsArticle> NewsArticle { get; set; }
    }
}
