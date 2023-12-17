using Blog.IServices;
using Blog.Models.Domain;
using Blog.Models.DTO;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
using AutoMapper;

namespace Blog.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthenticationService(SignInManager<ApplicationUser> signInManager, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._mapper = mapper;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null) {
                status.StatusCode = 0;
                status.Message = "Invalid Username";
                return status;
            } 
            // check if password match
            if(!await userManager.CheckPasswordAsync(user, model.Password)) {
                status.StatusCode = 0;
                status.Message = "Invalid Password";
                return status;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signInResult.Succeeded) {
                var userRoles = await userManager.GetRolesAsync(user);
                var autoClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                //AuthenticationProperties properties = new AuthenticationProperties()
                //{
                //    AllowRefresh = true,
                //    IsPersistent = model.KeepLoggedIn
                //};
                foreach (var userRole in userRoles)
                {
                    autoClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Logged in Successfully";
                return status;
            }
            if (signInResult.IsLockedOut) {
                status.StatusCode = 0;
                status.Message = "User Locked Out";
                return status;
            }
            {
                status.StatusCode = 0;
                status.Message = "Error on loggin In";
                //TempData["error"] = status.Message;
                return status;
            }
        }


        //public List<Role> GetRoles()
        //{
        //    var roles = roleManager.Roles;
        //    var data = _mapper.Map<List<Role>>(roles);

        //    return data;
        //}

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public  async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.Username);
            var emailExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null || emailExists != null)
            {
                status.StatusCode = 0;
                status.Message = "User already exists";
                return status;
            }
            ApplicationUser user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                //check add phone
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Username,
                EmailConfirmed = true,
                //Id = Guid.NewGuid().ToString(),
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) 
            { 
                status.StatusCode= 0;
                status.Message = "User Creation Failed";
                  return status;
            }

            //userManager.SaveChanges(model);
            
            //role management
            if (!await roleManager.RoleExistsAsync(model.Role))
            {
                await roleManager.CreateAsync(new IdentityRole(model.Role));
            }

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 0;
            status.Message = "User has registered successfully";
            return status;

        }

        public async Task<Status> SaveUsersAsync(SaveUsers model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.Username);
            var emailExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null || emailExists != null)
            {
                status.StatusCode = 0;
                status.Message = "Admin already exists";
                return status;

            }
            ApplicationUser user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                //Role = model.Role,
                UserName = model.Username,
                EmailConfirmed = true,
                //Id = model.Id
                //model.Password = '00000',
                
            };
            
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Admin Creation Failed";
                return status;
            }

            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }
            status.StatusCode = 0;
            status.Message = "Admin created successfully";
            return status;
        }

        public async Task<Status> UpdateUsersAsync(UpdateUsers model)
        {
            var status = new Status();
            // get user
            //var userr = await userManager.Users.Where(x => x.Id == model.Id);
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Admin not found";
                return status;
            }

            var emailExists = await userManager.FindByEmailAsync(model.Email);

            if (emailExists != null)
            {
                status.StatusCode = 0;
                status.Message = "Email is already in use by another user";
                return status;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Username;
            //user.role = model.Role;

            if (!string.IsNullOrEmpty(model.SelectedRole))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, userRoles.ToArray());

                await userManager.AddToRoleAsync(user, model.SelectedRole);
            }
            var updateUser = await userManager.UpdateAsync(user);

            if (!updateUser.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Admin update failed";
                return status;
            }

            status.StatusCode = 1;
            status.Message = "Admin updated successfully";
            return status;
        }

        //public async Task<Status> DeleteUsersAsync(OtherUsers model)
        //{
        //    var user = userManager.Users.Where(x => x.Id == model.Id);

        //    //userManager.Users.DeleteAsync(user);
        //    await userManager.DeleteAsync(user);

        //}
    }
}
