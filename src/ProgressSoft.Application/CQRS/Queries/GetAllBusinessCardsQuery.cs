using MediatR;

using ProgressSoft.Domain.Entities;

namespace ProgressSoft.Application.CQRS.Queries;

public sealed record GetAllBusinessCardsQuery(
    string? Name,
    DateTime? DateOfBirth,
    string? Phone,
    string? Gender,
    string? Email,
    int? PageNumber,
    int? Size)
    : IRequest<GetAllBusinessCardsQueryResult>;

public sealed record GetAllBusinessCardsQueryResult(PaginationResult<BusinessCard> Result);
