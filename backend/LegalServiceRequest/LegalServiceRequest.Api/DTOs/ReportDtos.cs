namespace LegalServiceRequest.Api.DTOs;

public class DashboardReportDto
{
    public int TotalRequests { get; set; }

    public int OpenRequests { get; set; }

    public int InProgressRequests { get; set; }

    public int CompletedRequests { get; set; }

    public int HighPriorityRequests { get; set; }

    public int OverdueRequests { get; set; }
}

public class GroupedCountDto
{
    public string Label { get; set; } = string.Empty;

    public int Count { get; set; }
}