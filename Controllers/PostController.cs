using System.Linq;
using Blog.IServices;
using Blog.Models;
using Blog.Models.Domain;
using Blog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IPostService _service;

        //private readonly DatabaseContext _context;

        public PostController(IPostService service)
        {
            _service = service;
        }

        // GET: api/posts
        [HttpGet("get-posts")]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _service.GetPostsAsync();
            if (posts == null)
            {
                return Ok(posts); //retuen a view

            }
            return Ok(posts); //retuen a view
        }

        // GET: api/posts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _service.GetPostAsync(id);

            if (post == null)
            {
                return NotFound();
                //return RedirectToAction("Index");
            }

            return Ok(post); // return a view
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: api/posts
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPost = await _service.CreatePostAsync(viewModel);

            // Return the created post with a 201 Created status
            return CreatedAtAction(nameof(GetPost), new { id = createdPost.PostId }, createdPost);
       
        }

        // PUT: api/posts/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePost(int id, [FromBody] Post updatedPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdated = _service.UpdatePost(id, updatedPost);

            if (!isUpdated)
            {
                return NotFound();
                //return RedirectToAction(nameof);
            }

            return NoContent(); // return view

        }

        // DELETE: api/posts/{id}
        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            var isDeleted = _service.DeletePost(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent(); //return view
        }
    }
}

