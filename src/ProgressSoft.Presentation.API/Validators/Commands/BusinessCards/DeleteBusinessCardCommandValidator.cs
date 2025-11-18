using FluentValidation;

using ProgressSoft.Application.CQRS.Commands.BusinessCards;

namespace ProgressSoft.Presentation.API.Validators.Commands.BusinessCards;

public class DeleteBusinessCardCommandValidator : AbstractValidator<DeleteBusinessCardCommand>
{
    public DeleteBusinessCardCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .WithMessage("Id should not be null or empty");
    }
}
