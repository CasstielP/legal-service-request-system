using LegalServiceRequest.Api.Data;
using LegalServiceRequest.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegalServiceRequest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardReportDto>> GetDashboardReport()
    {
        var today = DateTime.UtcNow.Date;

        var report = new DashboardReportDto
        {
            TotalRequests = await _context.ServiceRequests.CountAsync(),

            OpenRequests = await _context.ServiceRequests
                .CountAsync(r => r.Status == "Open"),

            InProgressRequests = await _context.ServiceRequests
                .CountAsync(r => r.Status == "In Progress"),

            CompletedRequests = await _context.ServiceRequests
                .CountAsync(r => r.Status == "Completed"),

            HighPriorityRequests = await _context.ServiceRequests
                .CountAsync(r => r.Priority == "High" || r.Priority == "Urgent"),

            OverdueRequests = await _context.ServiceRequests
                .CountAsync(r =>
                    r.DueDate.HasValue &&
                    r.DueDate.Value.Date < today &&
                    r.Status != "Completed")
        };

        return Ok(report);
    }

    [HttpGet("by-status")]
    public async Task<ActionResult<IEnumerable<GroupedCountDto>>> GetRequestsByStatus()
    {
        var result = await _context.ServiceRequests
            .GroupBy(r => r.Status)
            .Select(group => new GroupedCountDto
            {
                Label = group.Key,
                Count = group.Count()
            })
            .OrderBy(item => item.Label)
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("by-department")]
    public async Task<ActionResult<IEnumerable<GroupedCountDto>>> GetRequestsByDepartment()
    {
        var result = await _context.ServiceRequests
            .GroupBy(r => r.Department)
            .Select(group => new GroupedCountDto
            {
                Label = group.Key,
                Count = group.Count()
            })
            .OrderBy(item => item.Label)
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("by-priority")]
    public async Task<ActionResult<IEnumerable<GroupedCountDto>>> GetRequestsByPriority()
    {
        var result = await _context.ServiceRequests
            .GroupBy(r => r.Priority)
            .Select(group => new GroupedCountDto
            {
                Label = group.Key,
                Count = group.Count()
            })
            .OrderBy(item => item.Label)
            .ToListAsync();

        return Ok(result);
    }
}