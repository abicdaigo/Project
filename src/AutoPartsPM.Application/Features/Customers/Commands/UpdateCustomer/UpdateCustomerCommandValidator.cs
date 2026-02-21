using FluentValidation;

namespace AutoPartsPM.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("有効な顧客IDを指定してください");

        RuleFor(x => x.CustomerCode)
            .NotEmpty().WithMessage("顧客コードは必須です")
            .MaximumLength(50).WithMessage("顧客コードは50文字以内で入力してください");

        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("顧客名は必須です")
            .MaximumLength(200).WithMessage("顧客名は200文字以内で入力してください");

        RuleFor(x => x.Email)
            .MaximumLength(200).WithMessage("メールアドレスは200文字以内で入力してください")
            .EmailAddress().WithMessage("有効なメールアドレスを入力してください")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
