using FluentValidation;

using ProgressSoft.Application.CQRS.Queries;

namespace ProgressSoft.Presentation.API.Validators.Queries;

public class ExportBusinessCardQueryValidator : AbstractValidator<ExportBusinessCardQuery>
{
    private static readonly string[] SupportedFormats = ["csv", "xml"];

    public ExportBusinessCardQueryValidator()
    {
        RuleFor(x => x.Format)
            .NotEmpty().WithMessage("Format query parameter is required.");

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");
    }
}
