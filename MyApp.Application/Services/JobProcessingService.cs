using MyApp.Domain.Interfaces;
using MyApp.Domain.Entities;

public class JobProcessingService
{
    private readonly IJobRepository _jobRepository;
    private readonly IJobExecutor _executor;

    public JobProcessingService(IJobRepository jobRepository, IJobExecutor executor)
    {
        _jobRepository = jobRepository;
        _executor = executor;
    }

    public async Task ProcessPendingJobsAsync()
    {
        var jobs = await _jobRepository.GetPendingJobsAsync();

        foreach (var job in jobs)
        {
            job.Status = "processing";
            job.UpdatedAt = DateTime.UtcNow;
            await _jobRepository.UpdateJobAsync(job);

            try
            {
                // 🔁 Agora chama o executor real
                var result = await _executor.ExecuteAsync(job);
                job.Status = "done";
                job.Result = result;
            }
            catch (Exception ex)
            {
                job.Status = "failed";
                job.Result = ex.Message;
            }

            job.UpdatedAt = DateTime.UtcNow;
            await _jobRepository.UpdateJobAsync(job);
        }
    }
}
