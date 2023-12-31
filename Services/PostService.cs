using AutoMapper;
using Blog.IServices;
using Blog.Models;
using Blog.Models.Domain;
using Blog.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services
{
    public class PostService : IPostService
    {
        private readonly IMapper mapper;
        private readonly DatabaseContext _context;


        public PostService(DatabaseContext context,IMapper mapper)
        {
            this.mapper = mapper;
            _context = context;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            var status = new Status();
            //var posts = await _context.Posts.Where(p => p.Status == StatusType).Include(p => p.Category).ToListAsync();
            return await _context.Posts.Include(p => p.Category).ToListAsync();
            //return posts;
        }

        public async Task<Post> GetPostAsync(int id)
        {
            return await _context.Posts.Include(p => p.Category).FirstOrDefaultAsync(p => p.PostId == id);
        }

        public async Task<Post> CreatePostAsync(PostViewModel viewModel)
        {

            var post = new Post
            {
                Title = viewModel.Title,
                Content = viewModel.Content,
                Status = viewModel.Status,
            };
            await _context.Posts.AddAsync(post); // Using AddAsync to add post to context
            await _context.SaveChangesAsync(); // Saving changes asynchronously

            return post;
        }

        public bool UpdatePost(int id, Post updatedPost)
        {
            var existingPost = _context.Posts.FirstOrDefault(p => p.PostId == id);

            if (existingPost == null)
            {
                return false; // Or throw an exception, depending on your design
            }

            //var map = mapper.Map<Post>(existingPost);

            existingPost.Title = updatedPost.Title;
            existingPost.Content = updatedPost.Content;
            existingPost.Author = updatedPost.Author;
            existingPost.ImageUrl = updatedPost.ImageUrl;
            existingPost.Status = updatedPost.Status;

            _context.SaveChanges();

            return true;
        }

        public bool DeletePost(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.PostId == id);

            if (post == null)
            {
                return false; // Or throw an exception, depending on your design
            }

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return true;
        }

    }
}
