using FluentValidation;

namespace AutoPartsPM.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
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
