using URL_Shortener.Data.Models;

namespace URL_Shortener.Services
{
    public interface IPostsViewingService
    {
        Task<PostViewingResult> ViewAboutPostAsync();
    }

    public class PostViewingResult
    {
        public Post? Post { get; private set; }

        public PostsOperationResultCode Status { get; private set; }

        public PostViewingResult(Post? post, PostsOperationResultCode status)
        {
            Post = post;
            Status = status;
        }
    }
}
