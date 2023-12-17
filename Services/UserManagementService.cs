using Blog.IServices;
using Blog.Models.Domain;
using Blog.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Services
{
    public class UserManagementService : IUserManagementService

    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserManagementService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
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

        //public async Task<Status<CreateUsersViewModel>> GetUsers(string userName)
        //{

        //    var status = new Status<CreateUsersViewModel>();
        //    if (string.IsNullOrEmpty(userName))
        //    {
        //        status.StatusCode = 0;
        //        status.Message = "No Username";
        //        return status; ;
        //    }

        //    var user = await userManager.FindByNameAsync(userName);

        //    if (user == null)
        //    {
        //        status.StatusCode = 0;
        //        status.Message = "User not found";
        //        return status; 
        //        //NotFound($"User with user name '{userName}' not found.");
        //    }

        //    var roles = await userManager.GetRolesAsync(user);
        //    var rolesNames = roles.ToList(); // Assuming RolesNames is a property in your model that holds role names.

        //    var viewModel = new CreateUsersViewModel
        //    {
        //        UserId = user.Id,
        //        UserName = user.UserName,
        //        Email = user.Email,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        SelectedRole = roles.FirstOrDefault()
        //        //SelectedRole = roles.FirstOrDefault()

        //        // Add other properties as needed
        //    };

        //    status.Data = viewModel;
        //    status.StatusCode = 1; // Indicate success
        //    status.Message = "User found";

        //    return status;
        //}
        public async Task<Status> UpdateUsersAsync(UpdateUsers model)
        {
            var status = new Status();
            //var userId = model.Id.ToString();
            // get user
            //var userr = await userManager.Users.Where(x => x.Id == model.Id);
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Admin not found";
                return status;
            }

            // Check if the provided email is already in use by another user
            if (!string.IsNullOrEmpty(model.Email) && !string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailExists = await userManager.FindByEmailAsync(model.Email);

                if (emailExists != null)
                {
                    status.StatusCode = 0;
                    status.Message = "Email is already in use by another user";
                    return status;
                }
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

    }
}
