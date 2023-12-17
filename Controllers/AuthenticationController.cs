using Blog.IServices;
using Blog.Models;
using Blog.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNet.Identity;
using Blog.Models.Domain;
using System.Data;
using System.Security.Claims;

namespace Blog.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IServices.IAuthenticationService _service;

        public AuthenticationController(IServices.IAuthenticationService service)
        {
            this._service = service;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var allUsers = await _service.GetAllAsync();
        //    return View();
        //}
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Role = "User";
            var result = await _service.RegisterAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Register)); //check
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.LoginAsync(model);
            if (result.StatusCode == 1)
            {

                //if(model.role == "admin")
                TempData["msg"] = result.Message;
                return RedirectToAction("Index", "AdminDashboard"); //chech this 
            }
            else
            {
                TempData["error"] = result.Message;
                return RedirectToAction(nameof(Login)); //check
            }



        }


        [HttpPost]
        // check this out
        public async Task<IActionResult> SaveUsers(SaveUsers model)
        {
            var result = await _service.SaveUsersAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(SaveUsers));
        }

        [HttpPatch]

        public async Task<IActionResult> UpdateUsers(UpdateUsers model)
        {
            //var Admin = await _service.get()
            try
            {


                var result = await _service.UpdateUsersAsync(model);
                if (result.StatusCode == 1)
                {
                    // Successful update
                    return Ok(new { Message = result.Message });
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

            [Authorize]

            public async Task<IActionResult> Logout()
            {
                await _service.LogoutAsync();
                return RedirectToAction("Index", "Home"); //check this
            }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public async Task<IActionResult> Reg()
        //{
        //    var model = new RegistrationModel
        //    {
        //        Username = "admin1",
        //        FirstName = "Moro",
        //        LastName = "savage",
        //        Email = "morosavage@gmail.com",
        //        Password = "Admin@1234"


        //    };
        //    model.Role = "Admin";
        //    var result = await _service.RegisterAsync(model);
        //    TempData["msg"] = result.Message;
        //    return Ok(result);
        //}

    }
}

