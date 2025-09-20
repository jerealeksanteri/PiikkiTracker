using PiikkiTracker.Data;

namespace PiikkiTracker.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
        Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string searchTerm);
        Task<bool> DeleteAllUserJobsAsync(string userId);
        Task<bool> DeleteAllUserProductsAsync(string userId);
        Task<bool> ResetUserDataAsync(string userId);
        Task<bool> ResetUserPasswordAsync(string userId, string newPassword);
    }
}