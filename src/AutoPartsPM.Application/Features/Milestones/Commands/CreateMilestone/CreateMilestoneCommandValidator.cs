using FluentValidation;

namespace AutoPartsPM.Application.Features.Milestones.Commands.CreateMilestone;

public class CreateMilestoneCommandValidator : AbstractValidator<CreateMilestoneCommand>
{
    public CreateMilestoneCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .GreaterThan(0).WithMessage("プロジェクトを選択してください");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("マイルストーン名は必須です")
            .MaximumLength(200).WithMessage("マイルストーン名は200文字以内で入力してください");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("期限日は必須です");
    }
}
