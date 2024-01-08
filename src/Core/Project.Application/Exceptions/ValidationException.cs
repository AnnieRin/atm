using FluentValidation.Results;

namespace Project.Application.Exceptions;
public class ValidationException : ApplicationException
{
    public static string ErrorMessage { get; private set; }
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {

    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base(BuildErrorMessage(failures))
    {

    }

    private static string BuildErrorMessage(IEnumerable<ValidationFailure> failures)
    {
        var arr = failures.Select(x => $" {x.ErrorMessage}.");
        return "Validation failed: " + string.Join(string.Empty, arr);
    }
}
