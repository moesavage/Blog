using Blog.Models.DTO;

namespace Blog.IServices
{
    public interface IADashboardServices
    {

        Task<Status> SaveUsersAsync(SaveUsers model);
    }
}
