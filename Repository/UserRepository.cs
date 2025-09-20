using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PiikkiTracker.Data;
using PiikkiTracker.Repository.IRepository;

namespace PiikkiTracker.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users
                .Include(u => u.UserJobs)
                .Include(u => u.UserProducts)
                .Include(u => u.Transactions)
                .ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.Users
                .Include(u => u.UserJobs)
                .Include(u => u.UserProducts)
                .Include(u => u.Transactions)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return user;
            }
            throw new InvalidOperationException($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            // Prevent deleting the last admin
            if (await IsLastAdminAsync(userId))
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllUsersAsync();
            }

            return await _userManager.Users
                .Include(u => u.UserJobs)
                .Include(u => u.UserProducts)
                .Include(u => u.Transactions)
                .Where(u => u.FirstName.Contains(searchTerm) ||
                           u.LastName.Contains(searchTerm) ||
                           u.Email.Contains(searchTerm) ||
                           u.UserName.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<bool> DeleteAllUserJobsAsync(string userId)
        {
            try
            {
                var userJobs = await _context.UserJobs
                    .Where(uj => uj.UserId == userId)
                    .ToListAsync();

                if (userJobs.Any())
                {
                    _context.UserJobs.RemoveRange(userJobs);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAllUserProductsAsync(string userId)
        {
            try
            {
                var userProducts = await _context.UserProducts
                    .Where(up => up.UserId == userId)
                    .ToListAsync();

                if (userProducts.Any())
                {
                    _context.UserProducts.RemoveRange(userProducts);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetUserDataAsync(string userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Delete all user jobs
                await DeleteAllUserJobsAsync(userId);

                // Delete all user products
                await DeleteAllUserProductsAsync(userId);

                // Reset user balance
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.Balance = 0;
                    await _userManager.UpdateAsync(user);
                }

                // Delete all transactions for this user
                var transactions = await _context.Transactions
                    .Where(t => t.UserId == userId)
                    .ToListAsync();

                if (transactions.Any())
                {
                    _context.Transactions.RemoveRange(transactions);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> ResetUserPasswordAsync(string userId, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;

                // Remove the current password
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded) return false;

                // Add the new password
                var addPasswordResult = await _userManager.AddPasswordAsync(user, newPassword);
                return addPasswordResult.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> AddUserToRoleAsync(string userId, string role)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;

                var result = await _userManager.AddToRoleAsync(user, role);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveUserFromRoleAsync(string userId, string role)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;

                // Prevent removing admin role if this is the last admin
                if (role == "Admin" && await IsLastAdminAsync(userId))
                {
                    return false;
                }

                var result = await _userManager.RemoveFromRoleAsync(user, role);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsLastAdminAsync(string userId)
        {
            var adminCount = await GetAdminCountAsync();
            var userRoles = await GetUserRolesAsync(userId);

            return adminCount == 1 && userRoles.Contains("Admin");
        }

        public async Task<int> GetAdminCountAsync()
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
            return adminUsers.Count;
        }
    }
}