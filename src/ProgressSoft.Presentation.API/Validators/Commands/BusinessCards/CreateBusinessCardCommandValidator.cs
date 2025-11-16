using FluentValidation;

using ProgressSoft.Application.CQRS.Commands.BusinessCards;


namespace ProgressSoft.Presentation.API.Validators.Commands.BusinessCards;

public class CreateBusinessCardCommandValidator : AbstractValidator<CreateBusinessCardCommand>
{
    // Max 1MB size for the original photo.
    // Base64 encoding inflates the size by ~33%. 
    // Max Base64 Size ≈ (1 MB * 4/3) ≈ 1,333,333 bytes. We'll use a slightly safer upper limit.
    private const int MaxPhotoBase64Bytes = 1_400_000;

    // Max length for a string column (matching the EF Core configuration used earlier)
    private const int NameMaxLength = 150;
    private const int GenderMaxLength = 50;
    private const int PhoneMaxLength = 50;
    private const int AddressMaxLength = 500;
    private const int EmailMaxLength = 256;

    public CreateBusinessCardCommandValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Name is required.")
           .MaximumLength(NameMaxLength)
           .WithMessage($"Name cannot exceed {NameMaxLength} characters.");

        RuleFor(x => x.Gender)
            .NotEmpty()
            .WithMessage("Gender is required.")
            .MaximumLength(GenderMaxLength)
            .WithMessage($"Gender cannot exceed {GenderMaxLength} characters.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required.")
            .MaximumLength(AddressMaxLength)
            .WithMessage($"Address cannot exceed {AddressMaxLength} characters.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .MaximumLength(PhoneMaxLength)
            .WithMessage($"Phone number cannot exceed {PhoneMaxLength} characters.")
            .Matches(@"^\+?[0-9\s\-\(\)]{8,20}$") // Simple regex for common international phone formats
            .WithMessage("Phone number format is invalid.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .MaximumLength(EmailMaxLength)
            .WithMessage($"Email cannot exceed {EmailMaxLength} characters.")
            .EmailAddress()
            .WithMessage("Email address is not in a valid format.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage("Date of Birth is required.")
            .Must(BeAValidAge)
            .WithMessage("Date of Birth must be in the past and represent a reasonable age (e.g., maximum 120 years old).");

        // --- 2. Optional Photo Validation (Size Check) ---

        When(x => !string.IsNullOrEmpty(x.PhotoBase64), () =>
        {
            // The value is present, now validate its format and size.
            RuleFor(x => x.PhotoBase64!)
                .Must(BeValidBase64)
                .WithMessage("Photo must be a valid Base64 string.")
                // Check if the Base64 string length exceeds the allowed size limit (1MB original data)
                .Must(s => s.Length <= MaxPhotoBase64Bytes)
                .WithMessage($"The photo size exceeds the maximum allowed limit of 1MB (Base64 string size max: {MaxPhotoBase64Bytes} characters).");
        });
    }

    private static bool BeAValidAge(DateTime dob)
    {
        // Must be in the past
        if (dob > DateTime.Today)
            return false;

        // Check if the person is reasonably aged (e.g., not older than 120)
        int age = DateTime.Today.Year - dob.Year;
        if (dob.Date > DateTime.Today.AddYears(-age))
            age--;

        return age >= 0 && age <= 120;
    }

    private static bool BeValidBase64(string base64String)
    {
        // Simple Base64 check: length is multiple of 4, or contains valid padding ('=')
        if ((base64String.Length % 4) != 0)
        {
            return false;
        }

        // Try parsing the string to byte array (the most reliable check)
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
