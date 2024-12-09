using Microsoft.EntityFrameworkCore;
using PiikkiTracker.Data;
using PiikkiTracker.Repository.IRepository;

namespace PiikkiTracker.Repository
{
    public class UserJobRepository : IUserJobRepository
    {
        private readonly ApplicationDbContext _db;

        public UserJobRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<UserJob> CreateUserJobAsync(UserJob userJob)
        {
            await _db.UserJobs.AddAsync(userJob);
            await _db.SaveChangesAsync();
            return userJob;
        }

        public async Task<bool> DeleteUserJobAsync(int userJobId)
        {
            var obj = _db.UserJobs.FirstOrDefaultAsync(u => u.Id == userJobId);
            if (await obj != null)
            {
                _db.UserJobs.Remove(await obj);
                return await _db.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<UserJob>> GetAllUserJobsAsync()
        {
            return await _db.UserJobs.ToListAsync();
        }

        public async Task<UserJob> GetUserJobByIdAsync(int userJobId)
        {
            var obj = _db.UserJobs.FirstOrDefaultAsync(u => u.Id == userJobId);
            if (await obj == null)
            {
                return new UserJob();
            }
            return await obj;
        }

        public async Task<UserJob> UpdateUserJobAsync(UserJob userJob)
        {
            var obj = await _db.UserJobs.FirstOrDefaultAsync(u => u.Id == userJob.Id);
            if (obj != null)
            {
                obj.UserId = userJob.UserId;
                obj.JobId = userJob.JobId;
                obj.Event = userJob.Event;
                obj.Description = userJob.Description;
                obj.IsAccepted = userJob.IsAccepted;
                obj.CreatedDate = userJob.CreatedDate;
                await _db.SaveChangesAsync();
            }
            return obj;
        }



    }
}
