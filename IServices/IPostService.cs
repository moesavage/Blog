using Blog.Models;
using Blog.Models.DTO;

namespace Blog.IServices
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post> GetPostAsync(int id);
        Task<Post> CreatePostAsync(PostViewModel viewModel);
        bool UpdatePost(int id, Post updatedPost);

        bool DeletePost(int id);

    }
}
