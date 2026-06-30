namespace LegalServiceRequest.Api.Models;

public class ServiceRequest
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Priority { get; set; } = "Medium";

    public string Status { get; set; } = "Open";

    public int? AssignedToUserId { get; set; }

    public User? AssignedToUser { get; set; }

    public int CreatedByUserId { get; set; }

    public User? CreatedByUser { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}