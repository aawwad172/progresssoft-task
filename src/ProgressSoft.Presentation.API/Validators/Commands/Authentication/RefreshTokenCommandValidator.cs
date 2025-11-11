using ProgressSoft.Application.CQRS.Commands.Authentication;

using FluentValidation;

namespace ProgressSoft.Presentation.API.Validators.Commands.Authentication;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}
