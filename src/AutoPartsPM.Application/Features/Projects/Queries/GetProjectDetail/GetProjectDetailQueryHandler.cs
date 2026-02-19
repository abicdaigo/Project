using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Projects.Queries.GetProjectDetail;

public class GetProjectDetailQueryHandler : IRequestHandler<GetProjectDetailQuery, ProjectDetailDto?>
{
    private readonly IApplicationDbContext _context;

    public GetProjectDetailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectDetailDto?> Handle(GetProjectDetailQuery request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Customer)
            .Include(p => p.Phases).ThenInclude(ph => ph.Deliverables)
            .Include(p => p.GateReviews)
            .Include(p => p.PPAPDocuments)
            .Include(p => p.Milestones)
            .Include(p => p.Issues)
            .Include(p => p.Equipment)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (project is null) return null;

        return new ProjectDetailDto
        {
            Id = project.Id,
            ProjectCode = project.ProjectCode,
            ProjectName = project.ProjectName,
            CustomerId = project.CustomerId,
            CustomerName = project.Customer.CustomerName,
            PartNumber = project.PartNumber,
            ModelCode = project.ModelCode,
            Status = project.Status,
            CurrentPhase = project.CurrentPhase,
            SOPDate = project.SOPDate,
            Description = project.Description,
            ProjectManagerId = project.ProjectManagerId,
            ProjectManagerName = project.ProjectManagerName,
            OverallProgress = project.Phases.Any()
                ? project.Phases.Average(ph => ph.CompletionRate) : 0,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            Phases = project.Phases.OrderBy(ph => ph.Phase).Select(ph => new PhaseDto
            {
                Id = ph.Id,
                Phase = ph.Phase,
                Status = ph.Status,
                PlannedStartDate = ph.PlannedStartDate,
                PlannedEndDate = ph.PlannedEndDate,
                ActualStartDate = ph.ActualStartDate,
                ActualEndDate = ph.ActualEndDate,
                CompletionRate = ph.CompletionRate,
                DeliverableCount = ph.Deliverables.Count,
                CompletedDeliverableCount = ph.Deliverables.Count(d => d.Status == DeliverableStatus.Completed)
            }).ToList(),
            GateReviews = project.GateReviews.OrderBy(g => g.GateNumber).Select(g => new GateReviewDto
            {
                Id = g.Id,
                GateNumber = g.GateNumber,
                ReviewDate = g.ReviewDate,
                Result = g.Result,
                ReviewerName = g.ReviewerName,
                Notes = g.Notes
            }).ToList(),
            PPAPDocuments = project.PPAPDocuments.OrderBy(d => d.ElementNumber).Select(d => new PPAPDocumentDto
            {
                Id = d.Id,
                ElementNumber = d.ElementNumber,
                ElementName = d.ElementName,
                Status = d.Status,
                Required = d.Required,
                FileName = d.FileName,
                ApprovedDate = d.ApprovedDate
            }).ToList(),
            Milestones = project.Milestones.OrderBy(m => m.DueDate).Select(m => new MilestoneDto
            {
                Id = m.Id,
                Name = m.Name,
                DueDate = m.DueDate,
                ActualDate = m.ActualDate,
                Status = m.Status,
                Phase = m.Phase
            }).ToList(),
            Issues = project.Issues.OrderByDescending(i => i.Priority).Select(i => new IssueDto
            {
                Id = i.Id,
                Title = i.Title,
                Priority = i.Priority,
                Status = i.Status,
                Category = i.Category,
                AssigneeName = i.AssigneeName,
                DueDate = i.DueDate
            }).ToList(),
            Equipment = project.Equipment.OrderBy(e => e.Name).Select(e => new EquipmentDto
            {
                Id = e.Id,
                EquipmentCode = e.EquipmentCode,
                Name = e.Name,
                Type = e.Type,
                Status = e.Status,
                DeliveryDate = e.DeliveryDate
            }).ToList(),
            OpenIssueCount = project.Issues.Count(i => i.Status == IssueStatus.Open || i.Status == IssueStatus.InProgress),
            PPAPCompletedCount = project.PPAPDocuments.Count(d => d.Status == DocumentStatus.Approved),
            PPAPTotalCount = project.PPAPDocuments.Count(d => d.Required)
        };
    }
}
