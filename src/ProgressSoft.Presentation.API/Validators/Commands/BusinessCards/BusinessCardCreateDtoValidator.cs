using System;

using FluentValidation;

using ProgressSoft.Domain.DTOs;

namespace ProgressSoft.Presentation.API.Validators.Commands.BusinessCards;

public class BusinessCardCreateDtoValidator : AbstractValidator<BusinessCardCreateDto>
{
    // Max 1MB size for the original photo.
    private const int MaxPhotoBase64Bytes = 1_400_000;

    // Max length for a string column
    private const int NameMaxLength = 150;
    private const int GenderMaxLength = 50;
    private const int PhoneMaxLength = 50;
    private const int AddressMaxLength = 500;
    private const int EmailMaxLength = 256;

    public BusinessCardCreateDtoValidator()
    {
        // --- All rules from the Command Validator are duplicated here ---

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(NameMaxLength).WithMessage($"Name cannot exceed {NameMaxLength} characters.");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .MaximumLength(GenderMaxLength).WithMessage($"Gender cannot exceed {GenderMaxLength} characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(AddressMaxLength).WithMessage($"Address cannot exceed {AddressMaxLength} characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(PhoneMaxLength).WithMessage($"Phone number cannot exceed {PhoneMaxLength} characters.")
            .Matches(@"^\+?[0-9\s\-\(\)]{8,20}$").WithMessage("Phone number format is invalid.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(EmailMaxLength).WithMessage($"Email cannot exceed {EmailMaxLength} characters.")
            .EmailAddress().WithMessage("Email address is not in a valid format.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of Birth is required.")
            .Must(BeAValidAge).WithMessage("Date of Birth must be in the past and represent a reasonable age.");

        // --- Optional Photo Validation ---
        When(x => !string.IsNullOrEmpty(x.PhotoBase64), () =>
        {
            RuleFor(x => x.PhotoBase64!)
                .Must(BeValidBase64).WithMessage("Photo must be a valid Base64 string.")
                .Must(s => s.Length <= MaxPhotoBase64Bytes)
                .WithMessage($"The photo size exceeds the maximum allowed limit of 1MB.");
        });
    }

    private static bool BeAValidAge(DateTime dob)
    {
        if (dob > DateTime.Today) return false;
        int age = DateTime.Today.Year - dob.Year;
        if (dob.Date > DateTime.Today.AddYears(-age)) age--;
        return age >= 0 && age <= 120;
    }

    private static bool BeValidBase64(string base64String)
    {
        if ((base64String.Length % 4) != 0) return false;
        try
        {
            Convert.FromBase64String(base64String);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}