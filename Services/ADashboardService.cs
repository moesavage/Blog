using Blog.IServices;
using Blog.Models.Domain;
using Blog.Models.DTO;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace Blog.Services
{
    public class ADashboardService : IADashboardServices
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ADashboardService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
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
                UserName = model.Username,
                //Id = 
                //Role = model.Role,
                //EmailConfirmed = true,
                //Password = '00000',

            };
            string defaultPassword = "User@101";

            //user.Id = Guid.NewGuid().ToString();

            var result = await userManager.CreateAsync(user, defaultPassword);
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
    }
}
