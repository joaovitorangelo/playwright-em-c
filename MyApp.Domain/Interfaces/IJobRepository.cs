using MyApp.Domain.Entities;

namespace MyApp.Domain.Interfaces;

public interface IJobRepository
{
    Task AddJobAsync(Job job);
    Task<List<Job>> GetPendingJobsAsync();       // 👈 novo
    Task UpdateJobAsync(Job job);                // 👈 novo
}
