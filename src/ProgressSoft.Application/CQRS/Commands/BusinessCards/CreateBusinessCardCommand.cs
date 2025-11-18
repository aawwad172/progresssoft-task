using MediatR;

using ProgressSoft.Domain.Entities;

namespace ProgressSoft.Application.CQRS.Commands.BusinessCards;

public sealed record CreateBusinessCardCommand(
    string Name,
    string Gender,
    DateTime DateOfBirth,
    string Email,
    string Phone,
    string Address,
    string? PhotoBase64) : IRequest<CreateBusinessCardCommandResult>;

public sealed record CreateBusinessCardCommandResult(BusinessCard Card);