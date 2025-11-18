using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ProgressSoft.Application.CQRS.Queries;
using ProgressSoft.Domain.Exceptions;
using ProgressSoft.Presentation.API.Interfaces;

namespace ProgressSoft.Presentation.API.Routes.BusinessCards;

public class ExportBusinessCards : IParameterizedQueryRoute<ExportBusinessCardsQuery>
{
    public static async Task<IResult> RegisterRoute(
        [AsParameters] ExportBusinessCardsQuery query,
        [FromServices] IMediator mediator,
        [FromServices] IValidator<ExportBusinessCardsQuery> validator)
    {
        // 1. Create the query object
        ExportBusinessCardsQuery exportQuery = new ExportBusinessCardsQuery(query.Format);

        // 2. Validate the query (e.g., is format "csv" or "xml"?)
        var validationResult = await validator.ValidateAsync(exportQuery);
        if (!validationResult.IsValid)
        {
            List<string> errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            // Use your custom exception middleware
            throw new CustomValidationException("Validation failed", errors);
        }

        // 3. Send to the handler
        var result = await mediator.Send(query);

        // 4. Return the file
        // Results.File() streams the byte array back to the client
        // with the specified content type and file name.
        return Results.File(
            result.FileContents,
            result.ContentType,
            result.FileName
        );
    }
}
