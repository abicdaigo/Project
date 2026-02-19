using FluentValidation;

namespace AutoPartsPM.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.ProjectCode)
            .NotEmpty().WithMessage("プロジェクトコードは必須です")
            .MaximumLength(50).WithMessage("プロジェクトコードは50文字以内で入力してください");

        RuleFor(x => x.ProjectName)
            .NotEmpty().WithMessage("プロジェクト名は必須です")
            .MaximumLength(200).WithMessage("プロジェクト名は200文字以内で入力してください");

        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("顧客を選択してください");

        RuleFor(x => x.PartNumber)
            .NotEmpty().WithMessage("品番は必須です")
            .MaximumLength(100).WithMessage("品番は100文字以内で入力してください");

        RuleFor(x => x.ProjectManagerId)
            .NotEmpty().WithMessage("プロジェクトマネージャーは必須です");
    }
}
