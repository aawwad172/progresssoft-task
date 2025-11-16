using FluentValidation;
using FluentValidation.Results;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ProgressSoft.Application.CQRS.Commands.BusinessCards;
using ProgressSoft.Domain.Exceptions;
using ProgressSoft.Presentation.API.Interfaces;
using ProgressSoft.Presentation.API.Models;

namespace ProgressSoft.Presentation.API.Routes.BusinessCards;

public class CreateBusinessCard : ICommandRoute<CreateBusinessCardCommand>
{
    public static async Task<IResult> RegisterRoute(
        [FromBody] CreateBusinessCardCommand request,
        [FromServices] IMediator mediator,
        [FromServices] IValidator<CreateBusinessCardCommand> validator)
    {
        ValidationResult validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            List<string> errors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

            // Throw a custom ValidationException that your middleware will catch
            throw new CustomValidationException("Validation failed", errors);
        }

        var result = await mediator.Send(request);

        return Results.Ok(ApiResponse<CreateBusinessCardCommandResult>.SuccessResponse(result));
    }
}
