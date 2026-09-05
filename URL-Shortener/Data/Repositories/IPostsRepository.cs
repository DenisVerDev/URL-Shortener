using URL_Shortener.Data.Models;

namespace URL_Shortener.Data.Repositories
{
    public interface IPostsRepository
    {
        Task<Post?> FindAboutPostAsync();
    }
}
