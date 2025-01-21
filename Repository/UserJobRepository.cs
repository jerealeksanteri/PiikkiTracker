using System.Data;
using Microsoft.AspNetCore.Identity;
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

            if (userJob.IsAccepted)
            {
                // Credit the User
                ApplicationUser? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userJob.UserId);
                Job? job = await _db.Jobs.FirstOrDefaultAsync(uj => uj.Id == userJob.JobId);

                if (user is null || job is null)
                {
                    throw new DataException("User or Job was not found");
                }

                user.Credit(job.Payment);

                _db.Users.Update(user);               

            }

            await _db.SaveChangesAsync();
            return userJob;
        }

        public async Task<bool> DeleteUserJobAsync(int userJobId)
        {
            var obj = await _db.UserJobs.FirstOrDefaultAsync(u => u.Id == userJobId);
            if (obj != null)
            {
                // Debit the User
                ApplicationUser? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == obj.UserId);
                if (user is null)
                {
                    throw new DataException("User Was Not found!");
                }

                user.Debit(obj.Job.Payment);

                _db.Users.Update(user);

                _db.UserJobs.Remove(obj);
                return await _db.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<UserJob>> GetAllUserJobsAsync()
        {
            return await _db.UserJobs
                .Include(uj => uj.Job)
                .Include(uj => uj.User)
                .ToListAsync();
        }

        public async Task<UserJob> GetUserJobByIdAsync(int userJobId)
        {
            var obj = _db.UserJobs
                .Include(uj => uj.Job)
                .Include(uj => uj.User)
                .FirstOrDefaultAsync(u => u.Id == userJobId);

            if (await obj == null)
            {
                return new UserJob();
            }
            return await obj;
        }

        public async Task<UserJob> UpdateUserJobAsync(UserJob userJob)
        {
            var obj = await _db.UserJobs.Include(uj => uj.Job).Include(uj => uj.User).FirstOrDefaultAsync(u => u.Id == userJob.Id);
            if (obj != null)
            {

                bool isBalanceCorrect = userJob.IsAccepted == obj.IsAccepted;

                if (!isBalanceCorrect)
                {
                    ApplicationUser? creditor = await _db.Users.FirstOrDefaultAsync(u => u.Id == userJob.UserId);
                    ApplicationUser? debtor = await _db.Users.FirstOrDefaultAsync(u => u.Id == obj.UserId);
                    Job? newJob = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == userJob.JobId);

                    if (creditor is null || debtor is null)
                    {
                        throw new DataException("Creditor or Debtor User is not Found");
                    }

                    if (newJob is null)
                    {
                        throw new DataException("New Job was not Found");
                    }


                    debtor.Debit(obj.Job.Payment);
                    creditor.Credit(newJob.Payment);

                    _db.Users.Update(debtor);
                    _db.Users.Update(creditor);


                }

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

        public async Task<IEnumerable<UserJob>> GetAllUnacceptedUserJobsAsync()
        {
            return await _db.UserJobs.Include(uj => uj.Job).Include(uj => uj.User).Where(u => u.IsAccepted == false).ToListAsync();
        }

        public async Task<IEnumerable<UserJob>> GetAllUserJobsByUserIdAsync(string userId)
        {
            return await _db.UserJobs.Include(uj => uj.Job).Include(uj => uj.User).Where(u => u.UserId == userId).ToListAsync();
        }


    }
}
