using System.Threading.Tasks;
using MyApp.Domain.Entities;

namespace MyApp.Domain.Interfaces;

public interface IJobExecutor
{
    Task<string> ExecuteAsync(Job job);
}
