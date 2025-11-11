using ProgressSoft.Application.CQRS.Commands.Authentication;
using ProgressSoft.Domain.Exceptions;
using ProgressSoft.Presentation.API.Interfaces;
using ProgressSoft.Presentation.API.Models;

using FluentValidation;
using FluentValidation.Results;

using MediatR;

namespace ProgressSoft.Presentation.API.Routes.Authentication;

public class RefreshToken : ICommandRoute<RefreshTokenCommand>
{
    public static async Task<IResult> RegisterRoute(
                   RefreshTokenCommand command,
                   IMediator mediator,
                   IValidator<RefreshTokenCommand> validator)
    {
        ValidationResult? validationResult = await validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            List<string> errors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

            // Throw a custom ValidationException that your middleware will catch
            throw new CustomValidationException("Validation failed", errors);
        }

        RefreshTokenCommandResult? response = await mediator.Send(command);
        return Results.Ok(
            ApiResponse<RefreshTokenCommandResult>.SuccessResponse(response));
    }
}
