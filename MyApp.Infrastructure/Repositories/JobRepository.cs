using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using MyApp.Infrastructure.Data;

namespace MyApp.Infrastructure.Repositories;

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _context;

    public JobRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddJobAsync(Job job)
    {
        await _context.Jobs.AddAsync(job);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Job>> GetPendingJobsAsync()
    {
        return await _context.Jobs
            .Where(j => j.Status == "pending")
            .ToListAsync();
    }

    public async Task UpdateJobAsync(Job job)
    {
        _context.Jobs.Update(job);
        await _context.SaveChangesAsync();
    }

    public async Task<Job?> GetByIdAsync(long id)
{
    return await _context.Jobs.FindAsync(id);
}

    // Outros métodos que você já tiver...
}
