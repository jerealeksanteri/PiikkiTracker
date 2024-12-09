using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PiikkiTracker.Data;
using PiikkiTracker.Repository.IRepository;

namespace PiikkiTracker.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _db;

        public JobRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Job> CreateJobAsync(Job job)
        {
            await _db.Jobs.AddAsync(job);
            await _db.SaveChangesAsync();

            return job;
        }
        public async Task<bool> DeleteJobAsync(int jobId)
        {
            var obj = _db.Jobs.FirstOrDefaultAsync(u => u.Id == jobId);

            if (await obj != null)
            {
                _db.Jobs.Remove(await obj);
                return await _db.SaveChangesAsync() > 0;
            }

            return false;


        }
        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            return await _db.Jobs.ToListAsync();
        }
        public async Task<Job> GetJobByIdAsync(int jobId)
        {
            var obj = _db.Jobs.FirstOrDefaultAsync(u => u.Id == jobId);
            
            if (await obj == null)
            {
                return new Job();
            }

            return await obj;
        }
        public async Task<Job> UpdateJobAsync(Job job)
        {
            var obj = await _db.Jobs.FirstOrDefaultAsync(u => u.Id == job.Id);

            if (obj != null)
            {
                obj.Name = job.Name;
                obj.Description = job.Description;
                obj.Payment = job.Payment;
                _db.Jobs.Update(obj);
                await _db.SaveChangesAsync();
                return obj;
            }

            return job;
        }
    }
}
