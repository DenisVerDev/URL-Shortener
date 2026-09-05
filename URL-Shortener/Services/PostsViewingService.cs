using URL_Shortener.Data.Repositories;

namespace URL_Shortener.Services
{
    public class PostsViewingService(IPostsRepository _pr) : IPostsViewingService
    {
        public virtual async Task<PostViewingResult> ViewAboutPostAsync()
        {
            var post = await _pr.FindPostAsync(1); // lets say About post will have id with value 1
            var status = post is null ? PostsOperationResultCode.AbsentPost : PostsOperationResultCode.Success;

            return new PostViewingResult(post, status);
        }
    }
}
