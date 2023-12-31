using Blog.IServices;
using Blog.Models;
using Blog.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;

        //private readonly DatabaseContext context;

        public CategoryController (ICategoryService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("get-categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _service.GetCategoriesAsync();
            //return Ok(categories);
            return View("Index", categories); //view 
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetCategory(int id)
        //{
        //    var category = await _service.GetCategoryAsync(id);
        //    if (category == null)
        //        return NotFound();
        //    return Ok(category);
        //}

        [HttpGet("Create")]
        public IActionResult CreateCategory()
        { 
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            var createdCategory = await _service.CreateCategoryAsync(category);
            //return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryId }, createdCategory);
            return View(createdCategory);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            var isUpdated = await _service.UpdateCategoryAsync(id, category);
            if (!isUpdated)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var isDeleted = await _service.DeleteCategoryAsync(id);
            if (!isDeleted)
                return NotFound();
            return NoContent();
        }
    }

}
