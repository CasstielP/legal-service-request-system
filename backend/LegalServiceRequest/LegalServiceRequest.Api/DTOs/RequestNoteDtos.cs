namespace LegalServiceRequest.Api.DTOs;

public class RequestNoteDto
{
    public int Id { get; set; }

    public int ServiceRequestId { get; set; }

    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string NoteText { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

public class CreateRequestNoteDto
{
    public int UserId { get; set; }

    public string NoteText { get; set; } = string.Empty;
}