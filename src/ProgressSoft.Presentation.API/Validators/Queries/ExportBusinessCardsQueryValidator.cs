using FluentValidation;

using ProgressSoft.Application.CQRS.Queries;

namespace ProgressSoft.Presentation.API.Validators.Queries;

public class ExportBusinessCardsQueryValidator : AbstractValidator<ExportBusinessCardsQuery>
{
    private static readonly string[] SupportedFormats = ["csv", "xml"];

    public ExportBusinessCardsQueryValidator()
    {
        RuleFor(x => x.Format)
            .NotEmpty().WithMessage("Format query parameter is required.");
    }
}
