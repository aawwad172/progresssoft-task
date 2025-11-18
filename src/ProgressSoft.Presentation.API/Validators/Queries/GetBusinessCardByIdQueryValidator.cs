using FluentValidation;

using ProgressSoft.Application.CQRS.Queries;

namespace ProgressSoft.Presentation.API.Validators.Queries;

public class GetBusinessCardByIdQueryValidator : AbstractValidator<GetBusinessCardByIdQuery>
{
    public GetBusinessCardByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .WithMessage("Id should not be null or empty");
    }
}
