using FluentValidation;
using Project.Domain.Enum;
using System.Text.RegularExpressions;

namespace Project.Application.Services.Transactions.Models;
public class TransactionModel
{
    public string UserIBAN { get; set; }
    public string? RecipientIBAN { get; set; }
    public decimal Amount { get; set; }
}
public class TransactionModelValidator : AbstractValidator<TransactionModel>
{
    public TransactionModelValidator()
    {
        RuleFor(x => x.UserIBAN)
          .NotEmpty().WithMessage("მიუთითეთ ანგარიშის ნომერი")
          .Must(BeValidAccountNumber).WithMessage("ანგარიშის ნომერი არასწორია");

        RuleFor(x => x.RecipientIBAN)
          .Must(BeValidAccountNumber).When(x => !string.IsNullOrEmpty(x.RecipientIBAN)).WithMessage("ადრესატის ანგარიშის ნომერი არასწორია");

        RuleFor(x => x.Amount)
          .GreaterThan(0).WithMessage("თანხა უნდა აღემატებოდეს 0-ს");
    }
    private bool BeValidAccountNumber(string accountNumber)
    {
        if (string.IsNullOrEmpty(accountNumber))
        {
            return false;
        }
        string pattern = @"^GE\d{2}TR\d{14}$";
        return Regex.IsMatch(accountNumber, pattern);
    }
}