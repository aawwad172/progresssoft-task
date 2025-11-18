using MediatR;

using ProgressSoft.Domain.Entities;

namespace ProgressSoft.Application.CQRS.Queries;

public sealed record GetBusinessCardByIdQuery(Guid Id) : IRequest<GetBusinessCardByIdQueryResult>;

public sealed record GetBusinessCardByIdQueryResult(BusinessCard Card);
