using Microsoft.Extensions.Logging;

using ProgressSoft.Application.CQRS.Queries;
using ProgressSoft.Domain.Entities;
using ProgressSoft.Domain.Exceptions;
using ProgressSoft.Domain.Interfaces.Application.Services;
using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

namespace ProgressSoft.Application.CQRS.QueryHandlers;

public class GetBusinessCardByIdQueryHandler(
    ICurrentUserService currentUserService,
    ILogger<GetBusinessCardByIdQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IRepository<BusinessCard> businessCardRepository)
    : BaseHandler<GetBusinessCardByIdQuery, GetBusinessCardByIdQueryResult>(currentUserService, logger, unitOfWork)
{
    private readonly IRepository<BusinessCard> _businessCardRepository = businessCardRepository;
    public override async Task<GetBusinessCardByIdQueryResult> Handle(
        GetBusinessCardByIdQuery request,
        CancellationToken cancellationToken)
    {
        BusinessCard? result = await _businessCardRepository.GetByIdAsync(request.Id);
        if (result is null)
            throw new NotFoundException($"Business Card with Id: {request.Id} not found");

        return new GetBusinessCardByIdQueryResult(result);
    }
}
