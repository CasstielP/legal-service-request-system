namespace LegalServiceRequest.Api.Models;

public class RequestStatusHistory
{
    public int Id { get; set; }

    public int ServiceRequestId { get; set; }

    public ServiceRequest? ServiceRequest { get; set; }

    public string OldStatus { get; set; } = string.Empty;

    public string NewStatus { get; set; } = string.Empty;

    public int ChangedByUserId { get; set; }

    public User? ChangedByUser { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}