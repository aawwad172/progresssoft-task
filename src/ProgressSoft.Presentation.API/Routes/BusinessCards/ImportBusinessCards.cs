using FluentValidation;
using FluentValidation.Results;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ProgressSoft.Application.CQRS.Commands.BusinessCards;
using ProgressSoft.Domain.DTOs;
using ProgressSoft.Domain.Exceptions;
using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;
using ProgressSoft.Presentation.API.Models;

namespace ProgressSoft.Presentation.API.Routes.BusinessCards;

public class ImportBusinessCards
{
    public static async Task<IResult> RegisterRoute(
        IFormFile file,
        [FromServices] IMediator mediator,
        [FromServices] IValidator<FileImportCommand> fileValidator,
        [FromServices] IValidator<BusinessCardCreateDto> cardValidator,
        [FromServices] IFileImportRepository fileImportRepository)
    {
        // 1. Initial Null Check
        if (file == null)
            throw new ArgumentNullException("No file has been uploaded");

        // 2. Validation of the File itself (size, type, emptiness)
        // 1. Validate the file BEFORE parsing
        FileImportCommand fileCommand = new(file);
        ValidationResult fileValidation = await fileValidator.ValidateAsync(fileCommand);

        // 2. Parse the file into card DTOs
        IFileImportRepository.ImportResult parsingResult = await fileImportRepository.ImportAsync(file.FileName, file.OpenReadStream());

        if (parsingResult == null || parsingResult.BusinessCards.Count == 0)
            throw new CustomValidationException("Import failed", ["No records found in file."]);

        // 3. Validate EACH parsed record (post-import validation)
        List<string> allErrors = [];
        int index = 1;

        foreach (var card in parsingResult.BusinessCards)
        {
            ValidationResult result = await cardValidator.ValidateAsync(card);
            if (!result.IsValid)
            {
                allErrors.AddRange(
                    result.Errors.Select(e => $"Row {index}: {e.ErrorMessage}")
                );
            }
            index++;
        }

        if (allErrors.Any())
            throw new CustomValidationException("Record validation failed", allErrors);

        // 4. Send command to application layer
        ImportBusinessCardsCommand command = new(parsingResult.BusinessCards);
        ImportBusinessCardsCommandResult resultFinal = await mediator.Send(command);

        return Results.Ok(
            ApiResponse<ImportBusinessCardsCommandResult>.SuccessResponse(resultFinal)
        );
    }
}
