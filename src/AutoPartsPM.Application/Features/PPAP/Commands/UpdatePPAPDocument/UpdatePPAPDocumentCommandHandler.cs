using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.PPAP.Commands.UpdatePPAPDocument;

public class UpdatePPAPDocumentCommandHandler : IRequestHandler<UpdatePPAPDocumentCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdatePPAPDocumentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdatePPAPDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _context.PPAPDocuments.FindAsync(new object[] { request.Id }, cancellationToken);

        if (document is null)
            return Result.Failure("PPAPドキュメントが見つかりません");

        document.Status = request.Status;
        document.SubmissionLevel = request.SubmissionLevel;
        document.Required = request.Required;
        document.Notes = request.Notes;
        document.ApprovedBy = request.ApprovedBy;

        if (request.Status == DocumentStatus.Approved && request.ApprovedDate is null)
            document.ApprovedDate = DateTime.UtcNow;
        else if (request.Status != DocumentStatus.Approved)
            document.ApprovedDate = null;
        else
            document.ApprovedDate = request.ApprovedDate;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
