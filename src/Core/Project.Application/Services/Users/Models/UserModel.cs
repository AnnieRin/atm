using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Project.Application.Services.Users.Models;
public class UserModel
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public int Age { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string PersonalN { get; set; }
}
public class UserRegistrationModelValidator : AbstractValidator<UserModel>
{
    public UserRegistrationModelValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("გთხოვთ, შეავსოთ სახელის ველი.")
            .Length(1, 50).WithMessage("სახელის სიგრძე არასწორია.");

        RuleFor(x => x.LastName)
             .NotEmpty().WithMessage("გთხოვთ, შეავსოთ გვარის ველი")
             .Length(1, 50).WithMessage("გვარის სიგრძე არასწორია");

        RuleFor(x => x.Age)
            .NotEmpty().WithMessage("შეავსეთ ასაკის ველი")
            .GreaterThan(17).WithMessage("რეგისტრაცია შესაძლებელია 18 წლის ასაკიდან");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("შეავსეთ Email-ის ველი");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("შეავსეთ UserName-ის ველი");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("გთხოვთ შეავსეთ პაროლის ველი");

        RuleFor(x => x.PersonalN)
            .NotEmpty().WithMessage("გთხოვთ, შეავსოთ პირადი ნომერი.")
            .Length(11).WithMessage("პირადი ნომრის სიგრძე არასწორია.")
            .Must(BeAllDigits).WithMessage("პირადი ნომერი უნდა შეიცავდეს მხოლოდ ციფრებს");

    }
    private bool BeAllDigits(string persoanlN)
    {
        return persoanlN.All(char.IsDigit);
    }
}
