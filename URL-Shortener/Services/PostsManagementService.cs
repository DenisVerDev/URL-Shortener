using Microsoft.IdentityModel.Tokens;
using URL_Shortener.Data.Models;
using URL_Shortener.Data.Repositories;

namespace URL_Shortener.Services
{
    public class PostsManagementService(IPostsRepository _pr) : IPostsManagementService
    {
        public virtual async Task<PostsOperationResultCode> UpdatePostAsync(Post post)
        {
            if(post is null)
                throw new ArgumentNullException($"{nameof(PostsManagementService)} cannot update null post!");

            if (post.Content.IsNullOrEmpty())
                throw new ArgumentException($"{nameof(PostsManagementService)} cannot update post with null or empty content!");

            await _pr.UpdatePostAsync(post);

            return PostsOperationResultCode.Success;
        }
    }
}
