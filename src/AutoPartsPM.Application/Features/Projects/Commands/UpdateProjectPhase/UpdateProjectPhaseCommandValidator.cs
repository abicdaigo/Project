using FluentValidation;

namespace AutoPartsPM.Application.Features.Projects.Commands.UpdateProjectPhase;

public class UpdateProjectPhaseCommandValidator : AbstractValidator<UpdateProjectPhaseCommand>
{
    public UpdateProjectPhaseCommandValidator()
    {
        RuleFor(x => x.Progress)
            .InclusiveBetween(0, 100).WithMessage("進捗率は0〜100の範囲で入力してください");
    }
}
