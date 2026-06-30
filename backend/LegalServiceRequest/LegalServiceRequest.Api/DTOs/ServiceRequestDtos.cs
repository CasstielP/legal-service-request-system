namespace LegalServiceRequest.Api.DTOs;

public class ServiceRequestListDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string? AssignedToUserName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DueDate { get; set; }
}

public class ServiceRequestDetailDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public int? AssignedToUserId { get; set; }

    public string? AssignedToUserName { get; set; }

    public int CreatedByUserId { get; set; }

    public string? CreatedByUserName { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class CreateServiceRequestDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Priority { get; set; } = "Medium";

    public int CreatedByUserId { get; set; }

    public int? AssignedToUserId { get; set; }

    public DateTime? DueDate { get; set; }
}

public class UpdateServiceRequestDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Priority { get; set; } = "Medium";

    public string Status { get; set; } = "Open";

    public int? AssignedToUserId { get; set; }

    public DateTime? DueDate { get; set; }
}