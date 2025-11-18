using MediatR;

using ProgressSoft.Domain.DTOs;

namespace ProgressSoft.Application.CQRS.Commands.BusinessCards;

public sealed record ImportBusinessCardsCommand(IEnumerable<BusinessCardCreateDto> Cards) : IRequest<ImportBusinessCardsCommandResult>;

public sealed record ImportBusinessCardsCommandResult(
    int TotalProcessed); // List of errors or failed records

