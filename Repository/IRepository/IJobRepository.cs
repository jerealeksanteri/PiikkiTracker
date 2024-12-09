using Microsoft.AspNetCore.Mvc;
using PiikkiTracker.Data;

namespace PiikkiTracker.Repository.IRepository
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetAllJobsAsync();
        Task<Job> GetJobByIdAsync(int jobId);
        Task<Job> CreateJobAsync(Job job);
        Task<Job> UpdateJobAsync(Job job);
        Task<bool> DeleteJobAsync(int jobId);
    }
}
