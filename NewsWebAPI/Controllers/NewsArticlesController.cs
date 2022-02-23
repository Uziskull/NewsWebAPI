#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Models;

namespace NewsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticlesController : ControllerBase
    {
        private readonly NewsWebAPIContext _context;

        public NewsArticlesController(NewsWebAPIContext context)
        {
            _context = context;
        }

        // GET: api/NewsArticles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> GetNewsArticle()
        {
            return await _context.NewsArticle.ToListAsync();
        }

        // GET: api/NewsArticles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsArticle>> GetNewsArticle(int id)
        {
            var newsArticle = await _context.NewsArticle.FindAsync(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            return newsArticle;
        }

        // PUT: api/NewsArticles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNewsArticle(int id, NewsArticle newsArticle)
        {
            if (id != newsArticle.Id)
            {
                return BadRequest();
            }

            _context.Entry(newsArticle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/NewsArticles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NewsArticle>> PostNewsArticle(NewsArticle newsArticle)
        {
            _context.NewsArticle.Add(newsArticle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNewsArticle", new { id = newsArticle.Id }, newsArticle);
        }

        // DELETE: api/NewsArticles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewsArticle(int id)
        {
            var newsArticle = await _context.NewsArticle.FindAsync(id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            _context.NewsArticle.Remove(newsArticle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NewsArticleExists(int id)
        {
            return _context.NewsArticle.Any(e => e.Id == id);
        }
    }
}
