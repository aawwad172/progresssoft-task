using FluentValidation;

using ProgressSoft.Application.CQRS.Commands.BusinessCards;

namespace ProgressSoft.Presentation.API.Validators.Commands.BusinessCards;

public class ImportBusinessCardsCommandValidator : AbstractValidator<ImportBusinessCardsCommand>
{
    private const int MaxBulkImportCount = 2000;

    public ImportBusinessCardsCommandValidator()
    {
        // 1. Ensure the list is not null
        RuleFor(x => x.Cards)
            .NotNull()
            .WithMessage("The list of cards for bulk import cannot be null.");

        // 2. Ensure the list is not empty
        RuleFor(x => x.Cards)
            .Must(cards => cards != null && cards.Any())
            .WithMessage("The import file was processed, but no valid business cards were extracted.");

        // 3. Ensure the total count is reasonable (limit to prevent abuse)
        RuleFor(x => x.Cards)
            .Must(cards => cards.Count() <= MaxBulkImportCount)
            .WithMessage($"The number of cards exceeds the maximum limit of {MaxBulkImportCount} per batch.");

        // 4. Validate each individual card inside the list
        // RuleForEach(x => x.Cards)
        //     .SetValidator(cardValidator);
    }
}
