using URL_Shortener.Data.Models;

namespace URL_Shortener.Services
{
    public interface IPostsManagementService
    {
        Task<PostsOperationResultCode> UpdatePostAsync(Post post);
    }

    public enum PostsOperationResultCode
    {
        Success,
        AbsentPost
    }
}
