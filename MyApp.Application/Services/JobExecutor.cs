using System;
using System.Threading.Tasks;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;

namespace MyApp.Application.Services;

public class JobExecutor : IJobExecutor
{
    public async Task<string> ExecuteAsync(Job job)
    {
        // Aqui entraria a lógica real do job
        await Task.Delay(1000); // Simula tarefa longa

        // Você pode usar job.PostId para personalizar o comportamento
        return $"Job {job.PostId} processed successfully";
    }
}
