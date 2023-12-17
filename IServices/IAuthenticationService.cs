using Blog.Models.Domain;
using Blog.Models.DTO;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;

namespace Blog.IServices
{
    public interface IAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);
        Task<Status> RegisterAsync(RegistrationModel model);
        Task<Status> SaveUsersAsync(SaveUsers model);
        Task<Status> UpdateUsersAsync(UpdateUsers model);
        //List<Role> GetRoles();

        //Task<Status> DeleteUsersAsync(OtherUsers model);
        Task LogoutAsync();
    }
}
