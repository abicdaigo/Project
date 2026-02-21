using FluentValidation;

namespace AutoPartsPM.Application.Features.Issues.Commands.CreateIssue;

public class CreateIssueCommandValidator : AbstractValidator<CreateIssueCommand>
{
    public CreateIssueCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .GreaterThan(0).WithMessage("プロジェクトを選択してください");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("タイトルは必須です")
            .MaximumLength(500).WithMessage("タイトルは500文字以内で入力してください");
    }
}
