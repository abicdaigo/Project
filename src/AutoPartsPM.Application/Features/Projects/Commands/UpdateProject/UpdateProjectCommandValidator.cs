using FluentValidation;

namespace AutoPartsPM.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.ProjectCode)
            .NotEmpty().WithMessage("プロジェクトコードは必須です")
            .MaximumLength(50).WithMessage("プロジェクトコードは50文字以内で入力してください");

        RuleFor(x => x.ProjectName)
            .NotEmpty().WithMessage("プロジェクト名は必須です")
            .MaximumLength(200).WithMessage("プロジェクト名は200文字以内で入力してください");

        RuleFor(x => x.PartNumber)
            .NotEmpty().WithMessage("品番は必須です")
            .MaximumLength(100).WithMessage("品番は100文字以内で入力してください");
    }
}
