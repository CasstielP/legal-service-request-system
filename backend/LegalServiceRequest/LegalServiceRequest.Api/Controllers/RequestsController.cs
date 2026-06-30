using LegalServiceRequest.Api.Data;
using LegalServiceRequest.Api.DTOs;
using LegalServiceRequest.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegalServiceRequest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    private readonly AppDbContext _context;

    public RequestsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceRequestListDto>>> GetRequests()
    {
        var requests = await _context.ServiceRequests
            .Include(r => r.AssignedToUser)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ServiceRequestListDto
            {
                Id = r.Id,
                Title = r.Title,
                Department = r.Department,
                Priority = r.Priority,
                Status = r.Status,
                AssignedToUserName = r.AssignedToUser != null ? r.AssignedToUser.FullName : null,
                CreatedAt = r.CreatedAt,
                DueDate = r.DueDate
            })
            .ToListAsync();

        return Ok(requests);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceRequestDetailDto>> GetRequestById(int id)
    {
        var request = await _context.ServiceRequests
            .Include(r => r.AssignedToUser)
            .Include(r => r.CreatedByUser)
            .Where(r => r.Id == id)
            .Select(r => new ServiceRequestDetailDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Department = r.Department,
                Priority = r.Priority,
                Status = r.Status,
                AssignedToUserId = r.AssignedToUserId,
                AssignedToUserName = r.AssignedToUser != null ? r.AssignedToUser.FullName : null,
                CreatedByUserId = r.CreatedByUserId,
                CreatedByUserName = r.CreatedByUser != null ? r.CreatedByUser.FullName : null,
                DueDate = r.DueDate,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (request == null)
        {
            return NotFound();
        }

        return Ok(request);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceRequestDetailDto>> CreateRequest(CreateServiceRequestDto dto)
    {
        var createdByUserExists = await _context.Users.AnyAsync(u => u.Id == dto.CreatedByUserId);

        if (!createdByUserExists)
        {
            return BadRequest("CreatedByUserId does not match an existing user.");
        }

        if (dto.AssignedToUserId.HasValue)
        {
            var assignedUserExists = await _context.Users.AnyAsync(u => u.Id == dto.AssignedToUserId.Value);

            if (!assignedUserExists)
            {
                return BadRequest("AssignedToUserId does not match an existing user.");
            }
        }

        var request = new ServiceRequest
        {
            Title = dto.Title,
            Description = dto.Description,
            Department = dto.Department,
            Priority = dto.Priority,
            Status = "Open",
            CreatedByUserId = dto.CreatedByUserId,
            AssignedToUserId = dto.AssignedToUserId,
            DueDate = dto.DueDate,
            CreatedAt = DateTime.UtcNow
        };

        _context.ServiceRequests.Add(request);
        await _context.SaveChangesAsync();

        var result = await _context.ServiceRequests
            .Include(r => r.AssignedToUser)
            .Include(r => r.CreatedByUser)
            .Where(r => r.Id == request.Id)
            .Select(r => new ServiceRequestDetailDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Department = r.Department,
                Priority = r.Priority,
                Status = r.Status,
                AssignedToUserId = r.AssignedToUserId,
                AssignedToUserName = r.AssignedToUser != null ? r.AssignedToUser.FullName : null,
                CreatedByUserId = r.CreatedByUserId,
                CreatedByUserName = r.CreatedByUser != null ? r.CreatedByUser.FullName : null,
                DueDate = r.DueDate,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            })
            .FirstAsync();

        return CreatedAtAction(nameof(GetRequestById), new { id = request.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRequest(int id, UpdateServiceRequestDto dto)
    {
        var request = await _context.ServiceRequests.FindAsync(id);

        if (request == null)
        {
            return NotFound();
        }

        if (dto.AssignedToUserId.HasValue)
        {
            var assignedUserExists = await _context.Users.AnyAsync(u => u.Id == dto.AssignedToUserId.Value);

            if (!assignedUserExists)
            {
                return BadRequest("AssignedToUserId does not match an existing user.");
            }
        }

        var oldStatus = request.Status;

        request.Title = dto.Title;
        request.Description = dto.Description;
        request.Department = dto.Department;
        request.Priority = dto.Priority;
        request.Status = dto.Status;
        request.AssignedToUserId = dto.AssignedToUserId;
        request.DueDate = dto.DueDate;
        request.UpdatedAt = DateTime.UtcNow;

        if (oldStatus != dto.Status)
        {
            var statusHistory = new RequestStatusHistory
            {
                ServiceRequestId = request.Id,
                OldStatus = oldStatus,
                NewStatus = dto.Status,
                ChangedByUserId = request.CreatedByUserId,
                ChangedAt = DateTime.UtcNow
            };

            _context.RequestStatusHistories.Add(statusHistory);
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRequest(int id)
    {
        var request = await _context.ServiceRequests.FindAsync(id);

        if (request == null)
        {
            return NotFound();
        }

        _context.ServiceRequests.Remove(request);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}