using LegalServiceRequest.Api.Data;
using LegalServiceRequest.Api.DTOs;
using LegalServiceRequest.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegalServiceRequest.Api.Controllers;

[ApiController]
[Route("api/requests/{requestId:int}/notes")]
public class RequestNotesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RequestNotesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RequestNoteDto>>> GetNotesForRequest(int requestId)
    {
        var requestExists = await _context.ServiceRequests
            .AnyAsync(r => r.Id == requestId);

        if (!requestExists)
        {
            return NotFound("Service request not found.");
        }

        var notes = await _context.RequestNotes
            .Where(n => n.ServiceRequestId == requestId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new RequestNoteDto
            {
                Id = n.Id,
                ServiceRequestId = n.ServiceRequestId,
                UserId = n.UserId,
                UserName = n.User != null ? n.User.FullName : null,
                NoteText = n.NoteText,
                CreatedAt = n.CreatedAt
            })
            .ToListAsync();

        return Ok(notes);
    }

    [HttpPost]
    public async Task<ActionResult<RequestNoteDto>> CreateNoteForRequest(
        int requestId,
        CreateRequestNoteDto dto)
    {
        var requestExists = await _context.ServiceRequests
            .AnyAsync(r => r.Id == requestId);

        if (!requestExists)
        {
            return NotFound("Service request not found.");
        }

        var userExists = await _context.Users
            .AnyAsync(u => u.Id == dto.UserId);

        if (!userExists)
        {
            return BadRequest("UserId does not match an existing user.");
        }

        if (string.IsNullOrWhiteSpace(dto.NoteText))
        {
            return BadRequest("Note text is required.");
        }

        var note = new RequestNote
        {
            ServiceRequestId = requestId,
            UserId = dto.UserId,
            NoteText = dto.NoteText.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.RequestNotes.Add(note);
        await _context.SaveChangesAsync();

        var result = await _context.RequestNotes
            .Where(n => n.Id == note.Id)
            .Select(n => new RequestNoteDto
            {
                Id = n.Id,
                ServiceRequestId = n.ServiceRequestId,
                UserId = n.UserId,
                UserName = n.User != null ? n.User.FullName : null,
                NoteText = n.NoteText,
                CreatedAt = n.CreatedAt
            })
            .FirstAsync();

        return CreatedAtAction(
            nameof(GetNotesForRequest),
            new { requestId = requestId },
            result);
    }
}