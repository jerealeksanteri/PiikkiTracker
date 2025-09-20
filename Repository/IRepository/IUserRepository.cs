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
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<bool> AddUserToRoleAsync(string userId, string role);
        Task<bool> RemoveUserFromRoleAsync(string userId, string role);
        Task<bool> IsLastAdminAsync(string userId);
        Task<int> GetAdminCountAsync();
        Task<IEnumerable<ApplicationUser>> GetTopSpendersAsync(int count = 5);
        Task<IEnumerable<ApplicationUser>> GetTopJobPerformersAsync(int count = 5);
        Task<IEnumerable<ApplicationUser>> GetTopBalanceHoldersAsync(int count = 5);
    }
}