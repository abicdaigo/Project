using AutoPartsPM.Domain.Enums;

namespace AutoPartsPM.Domain.Entities;

public class Project : BaseEntity
{
    public string ProjectCode { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string? ModelCode { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Draft;
    public APQPPhase CurrentPhase { get; set; } = APQPPhase.Planning;
    public DateTime? SOPDate { get; set; }
    public string? Description { get; set; }
    public string ProjectManagerId { get; set; } = string.Empty;
    public string? ProjectManagerName { get; set; }

    // Navigation
    public Customer Customer { get; set; } = null!;
    public ICollection<ProjectPhase> Phases { get; set; } = new List<ProjectPhase>();
    public ICollection<GateReview> GateReviews { get; set; } = new List<GateReview>();
    public ICollection<PPAPDocument> PPAPDocuments { get; set; } = new List<PPAPDocument>();
    public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
    public ICollection<Issue> Issues { get; set; } = new List<Issue>();
    public ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
    public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
}
