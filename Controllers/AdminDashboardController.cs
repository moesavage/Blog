using Blog.IServices;
using Blog.Models;
using Blog.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blogger.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly IADashboardServices _service;
        private readonly RoleManager<IdentityRole> _manager;


        public AdminDashboardController(IADashboardServices service, RoleManager<IdentityRole> roleManager)
        {
            this._service = service;
            _manager = roleManager;    
        }

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult UserManagement()
        //{
        //    return View();
        //}

        //public IActionResult CreateUsers()
        //{
        //    //ViewData["Title"] = "Create User";
        //    var roles = _manager.Roles;
        //    return View(roles);
        //    //return View();
        //}

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

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}