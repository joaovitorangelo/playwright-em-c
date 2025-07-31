using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyApp.Domain.Interfaces;

namespace MyApp.Application.Services
{
    public class JobProcessingService
    {
        private readonly IJobRepository _repository;
        private readonly IJobExecutor _executor;

        public JobProcessingService(IJobRepository repository, IJobExecutor executor)
        {
            _repository = repository;
            _executor = executor;
        }

        public async Task ProcessPendingJobsAsync()
        {
            var jobs = await _repository.GetPendingJobsAsync();

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "scraping", durable: false, exclusive: false, autoDelete: false);

            foreach (var job in jobs)
            {
                var payload = JsonSerializer.Serialize(new { job.Id, job.PostId });
                var body = Encoding.UTF8.GetBytes(payload);

                channel.BasicPublish(exchange: "", routingKey: "scraping", basicProperties: null, body: body);

                job.Status = "queued";
                await _repository.UpdateJobAsync(job);
            }
        }
    }
}
