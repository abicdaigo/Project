using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.PPAP.Commands.UpdatePPAPDocument;

public record UpdatePPAPDocumentCommand : IRequest<Result>
{
    public int Id { get; init; }
    public DocumentStatus Status { get; init; }
    public PPAPLevel SubmissionLevel { get; init; }
    public bool Required { get; init; }
    public string? Notes { get; init; }
    public string? ApprovedBy { get; init; }
    public DateTime? ApprovedDate { get; init; }
}
