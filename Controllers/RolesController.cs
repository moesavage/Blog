using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _manager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _manager = roleManager;

        }

        [HttpGet]
        public IActionResult Index()
        {
            var roles = _manager.Roles;
            return View(roles);
        }

        [HttpGet]
        public IActionResult Edit(string name)
        {
            if (name == null)
            {
                return BadRequest("Role name is required for editing.");
            }
            var role = _manager.Roles.SingleOrDefault(r => r.Name == name);

            //if (role == null)
            //{
            //    return NotFound($"Role with name '{name}' not found.");
            //}

            return View(role);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            //if (string.IsNullOrEmpty(role.Name))
            //{
            //    // Handle the case where role.Name is null or empty
            //    ModelState.AddModelError("Name", "Role name cannot be empty.");
            //    return View(role);
            //}
            if (! await _manager.RoleExistsAsync(role.Name))
            {
                RedirectToAction("Index");
            }
                    //.GetAwaiter().GetResult())
            {
                var result = await _manager.CreateAsync(new IdentityRole(role.Name));
                //.GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var role = await _manager.FindByIdAsync(model.Id);

                if (role == null)
                {
                    return NotFound($"Role with ID '{model.Id}' not found.");
                }

                // Update role properties based on the form data
                role.Name = model.Name;
                role.NormalizedName = model.NormalizedName;


                // Update other properties as needed

                var result = await _manager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                // If the update fails, handle the error (e.g., display an error message)
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If ModelState is not valid, redisplay the form with validation errors
            return View(model);
        }

    }
}
