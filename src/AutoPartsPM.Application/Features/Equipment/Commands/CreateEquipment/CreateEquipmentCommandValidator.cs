using FluentValidation;

namespace AutoPartsPM.Application.Features.Equipment.Commands.CreateEquipment;

public class CreateEquipmentCommandValidator : AbstractValidator<CreateEquipmentCommand>
{
    public CreateEquipmentCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .GreaterThan(0).WithMessage("プロジェクトを選択してください");

        RuleFor(x => x.EquipmentCode)
            .NotEmpty().WithMessage("設備コードは必須です")
            .MaximumLength(50).WithMessage("設備コードは50文字以内で入力してください");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("設備名は必須です")
            .MaximumLength(200).WithMessage("設備名は200文字以内で入力してください");
    }
}
