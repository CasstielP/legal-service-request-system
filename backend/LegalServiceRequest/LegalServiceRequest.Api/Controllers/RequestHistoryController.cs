using LegalServiceRequest.Api.Data;
using LegalServiceRequest.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegalServiceRequest.Api.Controllers;

[ApiController]
[Route("api/requests/{requestId:int}/history")]
public class RequestHistoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public RequestHistoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RequestStatusHistoryDto>>> GetHistoryForRequest(int requestId)
    {
        var requestExists = await _context.ServiceRequests
            .AnyAsync(r => r.Id == requestId);

        if (!requestExists)
        {
            return NotFound("Service request not found.");
        }

        var history = await _context.RequestStatusHistories
            .Where(h => h.ServiceRequestId == requestId)
            .OrderByDescending(h => h.ChangedAt)
            .Select(h => new RequestStatusHistoryDto
            {
                Id = h.Id,
                ServiceRequestId = h.ServiceRequestId,
                OldStatus = h.OldStatus,
                NewStatus = h.NewStatus,
                ChangedByUserId = h.ChangedByUserId,
                ChangedByUserName = h.ChangedByUser != null ? h.ChangedByUser.FullName : null,
                ChangedAt = h.ChangedAt
            })
            .ToListAsync();

        return Ok(history);
    }
}