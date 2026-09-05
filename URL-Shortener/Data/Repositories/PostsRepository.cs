using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data.Models;

namespace URL_Shortener.Data.Repositories
{
    public class PostsRepository (UShortDbContext _dbContext) : IPostsRepository
    {
        public virtual async Task<Post?> FindPostAsync(int id)
            => await _dbContext.Posts.FindAsync(id);

        public async Task UpdatePostAsync(Post post)
        {
            _dbContext.Posts.Update(post);
            await _dbContext.SaveChangesAsync();
        }
    }
}
