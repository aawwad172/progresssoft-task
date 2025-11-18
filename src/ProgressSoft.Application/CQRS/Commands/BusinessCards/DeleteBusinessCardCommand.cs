using MediatR;

namespace ProgressSoft.Application.CQRS.Commands.BusinessCards;

public sealed record DeleteBusinessCardCommand(Guid Id) : IRequest<DeleteBusinessCardCommandResult>;

public sealed record DeleteBusinessCardCommandResult(bool Deleted);