namespace LegalServiceRequest.Api.Models;

public class RequestNote
{
    public int Id { get; set; }

    public int ServiceRequestId { get; set; }

    public ServiceRequest? ServiceRequest { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }

    public string NoteText { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}