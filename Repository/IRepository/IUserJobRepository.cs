using PiikkiTracker.Data;

namespace PiikkiTracker.Repository.IRepository
{
    public interface IUserJobRepository
    {
        Task<IEnumerable<UserJob>> GetAllUserJobsAsync();
        Task<UserJob> GetUserJobByIdAsync(int userJobId);
        Task<UserJob> CreateUserJobAsync(UserJob userJob);
        Task<UserJob> UpdateUserJobAsync(UserJob userJob);
        Task<bool> DeleteUserJobAsync(int userJobId);
        Task<IEnumerable<UserJob>> GetAllUnacceptedUserJobsAsync();
        Task<IEnumerable<UserJob>> GetAllUserJobsByUserIdAsync(string userId);
    }
}
