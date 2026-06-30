namespace LegalServiceRequest.Api.DTOs;

public class RequestStatusHistoryDto
{
    public int Id { get; set; }

    public int ServiceRequestId { get; set; }

    public string OldStatus { get; set; } = string.Empty;

    public string NewStatus { get; set; } = string.Empty;

    public int ChangedByUserId { get; set; }

    public string? ChangedByUserName { get; set; }

    public DateTime ChangedAt { get; set; }
}