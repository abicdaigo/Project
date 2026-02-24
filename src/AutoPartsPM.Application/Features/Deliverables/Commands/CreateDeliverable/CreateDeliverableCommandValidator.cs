using FluentValidation;

namespace AutoPartsPM.Application.Features.Deliverables.Commands.CreateDeliverable;

public class CreateDeliverableCommandValidator : AbstractValidator<CreateDeliverableCommand>
{
    public CreateDeliverableCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("成果物名は必須です")
            .MaximumLength(200).WithMessage("成果物名は200文字以内で入力してください");

        RuleFor(x => x.ProjectPhaseId)
            .GreaterThan(0).WithMessage("フェーズIDが無効です");
    }
}
