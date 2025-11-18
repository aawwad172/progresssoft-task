using FluentValidation;
using FluentValidation.Results;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ProgressSoft.Application.CQRS.Queries;
using ProgressSoft.Domain.Exceptions;
using ProgressSoft.Presentation.API.Interfaces;

namespace ProgressSoft.Presentation.API.Routes.BusinessCards;

public class ExportBusinessCard : IParameterizedQueryRoute<ExportBusinessCardQuery>
{
    public static async Task<IResult> RegisterRoute(
        [AsParameters] ExportBusinessCardQuery query,
        [FromServices] IMediator mediator,
        [FromServices] IValidator<ExportBusinessCardQuery> validator)
    {
        ValidationResult validationResult = await validator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            List<string> errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            // Use your custom exception middleware
            throw new CustomValidationException("Validation failed", errors);
        }

        ExportBusinessCardQueryResult result = await mediator.Send(query);

        return Results.File(
            result.FileContents,
            result.ContentType,
            result.FileName
        );
    }
}
