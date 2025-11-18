using FluentValidation;

using ProgressSoft.Application.CQRS.Queries;

namespace ProgressSoft.Presentation.API.Validators.Queries;

public class GetAllBusinessCardsQueryValidator : AbstractValidator<GetAllBusinessCardsQuery>
{
    public GetAllBusinessCardsQueryValidator()
    {
        // PageNumber: Must be null or greater than zero
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .When(x => x.PageNumber.HasValue)
            .WithMessage("Page number must be a positive integer.");

        // Size: Must be null or greater than zero (and optionally cap it)
        RuleFor(x => x.Size)
            .GreaterThan(0)
            .When(x => x.Size.HasValue)
            .WithMessage("Page size must be a positive integer.");

        // --- Filtering Validation ---

        // DateOfBirth: If provided, ensure it's a valid date that is in the past.
        RuleFor(x => x.DateOfBirth)
            // You are checking DateOfBirth here, not Name as in your snippet.
            .LessThanOrEqualTo(DateTime.Now.Date)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of Birth cannot be in the future.");

        // Email: If provided, it must be a valid email format.
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Email must be in a valid format.");

        // Phone: If provided, ensure it contains only digits and is a reasonable length.
        RuleFor(x => x.Phone)
            .Matches(@"^[\d\s\-\(\)\+]+$") // Allows digits, spaces, hyphens, parentheses, and plus sign
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Phone number contains invalid characters.");

        // Gender: Simple length check if you expect predefined values like 'Male'/'Female'.
        RuleFor(x => x.Gender)
            .MaximumLength(15)
            .When(x => !string.IsNullOrEmpty(x.Gender))
            .WithMessage("Gender input is too long.");

        // Name: Simple length check for the filter value.
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Name))
            .WithMessage("Name filter criteria is too long.");
    }
}
