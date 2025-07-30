namespace MyApp.Domain.Entities;

public class Job
{
    public long Id { get; set; }
    public long PostId { get; set; }
    public string Status { get; set; } = "pending";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Result { get; set; }
}
