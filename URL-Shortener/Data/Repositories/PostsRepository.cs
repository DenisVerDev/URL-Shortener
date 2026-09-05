using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data.Models;

namespace URL_Shortener.Data.Repositories
{
    public class PostsRepository (UShortDbContext _dbContext) : IPostsRepository
    {
        public virtual async Task<Post?> FindAboutPostAsync()
            => await _dbContext.Posts.FirstOrDefaultAsync();
    }
}
