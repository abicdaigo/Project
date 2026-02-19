namespace AutoPartsPM.Domain.Entities;

public class ProjectMember : BaseEntity
{
    public int ProjectId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Department { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
}
