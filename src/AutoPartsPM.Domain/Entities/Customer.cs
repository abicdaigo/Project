namespace AutoPartsPM.Domain.Entities;

public class Customer : BaseEntity
{
    public string CustomerCode { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
