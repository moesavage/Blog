using Blog.Models.Domain;
using Blog.Models.DTO;


namespace Blog.IServices
{
    public interface IUserManagementService
    {
        Task<Status> SaveUsersAsync(SaveUsers model);
        Task<Status> UpdateUsersAsync(UpdateUsers model);
        //Task<Status<CreateUsersViewModel>> GetUsers(string userName);
    }
}
