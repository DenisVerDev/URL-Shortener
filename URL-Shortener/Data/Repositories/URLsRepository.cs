using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using URL_Shortener.Data.Models;

namespace URL_Shortener.Data.Repositories
{
    public class URLsRepository (UShortDbContext _dbContext) : IURLsRepository
    {
        public virtual async Task<URL> AddURLAsync(URL url)
        {
            _dbContext.URLs.Add(url);
            await _dbContext.SaveChangesAsync();

            return url;
        }

        public virtual async Task<URL> CreateURLAsync(string originalUrl, string shortUrlId, int creatorId)
        {
            var url = new URL
            {
                OriginalURL = originalUrl,
                ShortURLId = shortUrlId,
                CreatorId = creatorId
            };

            return await AddURLAsync(url);
        }

        public virtual async Task<URL?> FindURLAsync(int id)
            => await _dbContext.URLs.FindAsync(id);

        public virtual async Task<URL?> FindURLAsync(string originalUrl)
            => await _dbContext.URLs.FirstOrDefaultAsync(u => u.OriginalURL == originalUrl);

        public virtual async Task<URL?> FindURLAsync(Expression<Func<URL, bool>> predicate)
            => await _dbContext.URLs.FirstOrDefaultAsync(predicate);

        public virtual async Task<List<URL>> FindURLsAsync(int creatorId)
            => await _dbContext.URLs.Where(u => u.Creator.Id == creatorId).ToListAsync();

        public virtual async Task<List<URL>> FindURLsAsync(Expression<Func<URL, bool>> predicate)
            => await _dbContext.URLs.Where(predicate).ToListAsync();

        public virtual async Task<URL?> FirstURLAsync()
            => await _dbContext.URLs.FirstOrDefaultAsync();

        public virtual async Task<URL?> FirstURLAsync(int creatorId)
            => await _dbContext.URLs.FirstOrDefaultAsync(u => u.Creator.Id == creatorId);

        public virtual async Task<URL?> FirstURLAsync(Expression<Func<URL, bool>> predicate)
            => await _dbContext.URLs.FirstOrDefaultAsync(predicate);

        public virtual async Task<URL?> LastURLAsync()
            => await _dbContext.URLs.LastOrDefaultAsync();

        public virtual async Task<URL?> LastURLAsync(int creatorId)
            => await _dbContext.URLs.LastOrDefaultAsync(u=>u.Creator.Id == creatorId);

        public virtual async Task<URL?> LastURLAsync(Expression<Func<URL, bool>> predicate)
            => await _dbContext.URLs.LastOrDefaultAsync(predicate);

        public virtual async Task DeleteURLAsync(URL url)
        {
            _dbContext.URLs.Remove(url);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteURLsAsync(IEnumerable<URL> urls)
        {
            _dbContext.URLs.RemoveRange(urls);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<bool> AnyURLAsync(Expression<Func<URL, bool>> predicate)
            => await _dbContext.URLs.AnyAsync(predicate);
    }
}
