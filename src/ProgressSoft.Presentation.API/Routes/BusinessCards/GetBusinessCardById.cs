using FluentValidation;
using FluentValidation.Results;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ProgressSoft.Application.CQRS.Queries;
using ProgressSoft.Domain.Exceptions;
using ProgressSoft.Presentation.API.Interfaces;
using ProgressSoft.Presentation.API.Models;

namespace ProgressSoft.Presentation.API.Routes.BusinessCards;

public class GetBusinessCardById : IParameterizedQueryRoute<GetBusinessCardByIdQuery>
{
    public static async Task<IResult> RegisterRoute(
        [AsParameters] GetBusinessCardByIdQuery query,
        [FromServices] IMediator mediator,
        [FromServices] IValidator<GetBusinessCardByIdQuery> validator)
    {
        ValidationResult validationResult = await validator.ValidateAsync(query);

        if (!validationResult.IsValid)
        {
            List<string> errors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

            // Throw a custom ValidationException that your middleware will catch
            throw new CustomValidationException("Validation failed", errors);
        }

        GetBusinessCardByIdQueryResult result = await mediator.Send(query);

        return Results.Ok(ApiResponse<GetBusinessCardByIdQueryResult>.SuccessResponse(result));
    }
}
