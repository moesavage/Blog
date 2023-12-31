using Blog.IServices;
using Blog.Models;
using Blog.Models.Domain;
using Blog.Models.DTO;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class UserManagementController : Controller

    {
        private readonly IUserManagementService _service;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> _manager;



        public UserManagementController(IUserManagementService service , UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _service = service;
            this.userManager = userManager;
            _manager = roleManager;


        }

        //public IActionResult UserManagement()
        //{
        //    return View();
        //}
        public  async Task<IActionResult> Index()

        {
            var users = await userManager.Users.Include(u => u.Roles).ToListAsync(); // Fetch users synchronously

            var usersWithRoles = new List<UserWithRolesViewModel>();

            foreach (var user in users)
            {
                var rolesNames = user.Roles
                    .Select(userRole => _manager.Roles.FirstOrDefault(r => r.Id == userRole.RoleId)?.Name)
                    .Where(roleName => roleName != null)
                    .ToList();

                 usersWithRoles.Add(new UserWithRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    RolesNames = rolesNames
                });

            }

            return View(usersWithRoles);
        }

        public IActionResult CreateUsers()
        {
            var viewModel = new CreateUsersViewModel
            {
                Roles = _manager.Roles.ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        // check this out
        public async Task<IActionResult> SaveUsers(SaveUsers model)
        {
            var result = await _service.SaveUsersAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(CreateUsers));
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(string userName)
        {

            //var data = _service.GetUsers(userName);
            var data = await userManager.Users.Include(u => u.Roles).Where(roleName => roleName != null).SingleOrDefaultAsync(x => x.UserName == userName);
                    //.Where(roleName => roleName != null);
           if (data == null)
            {
                // Handle the case where user data is not found
                return NotFound($"User with user name '{userName}' not found.");
            }

            TempData["msg"] = $"User with user name '{userName}' found.";

            var roles = await userManager.GetRolesAsync(data);

            var viewModel = new CreateUsersViewModel
            {
                UserId = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                UserName = data.UserName,
                Email = data.Email,
                Roles = await _manager.Roles.ToListAsync(),
                SelectedRole = roles.FirstOrDefault()
            };

            //viewModel.UserId = viewModel.UserId.ToString();

            return View("EditUsers", viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUsers(UpdateUsers model)
        {
            //var Admin = await _service.get()
            try
            {
                var result = await _service.UpdateUsersAsync(model);
                if (result.StatusCode == 1)
                {
                    // Successful update
                    TempData["msg"] = result.Message;
                    return RedirectToAction("Index");
                        //Ok(new { Message = result.Message });
                }
                else
                {
                    // Failed update
                    return BadRequest(new { Message = result.Message });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Internal Server Error" });
            }

        }
    }

}
