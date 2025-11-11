using System.Linq.Expressions;

using Microsoft.Extensions.Logging;

using ProgressSoft.Application.CQRS.Queries;
using ProgressSoft.Application.Utilities.Extensions;
using ProgressSoft.Domain.Entities;
using ProgressSoft.Domain.Interfaces.Application.Services;
using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

namespace ProgressSoft.Application.CQRS.QueryHandlers;

public class GetAllBusinessCardsQueryHandler(
    ICurrentUserService currentUserService,
    ILogger<GetAllBusinessCardsQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IRepository<BusinessCard> businessCardRepository)
    : BaseHandler<GetAllBusinessCardsQuery, GetAllBusinessCardsQueryResult>(currentUserService, logger, unitOfWork)
{
    private readonly IRepository<BusinessCard> _businessCardRepository = businessCardRepository;

    public override async Task<GetAllBusinessCardsQueryResult> Handle(
        GetAllBusinessCardsQuery request,
        CancellationToken cancellationToken)
    {
        // Start with a true filter (select all)
        Expression<Func<BusinessCard, bool>> filter = card => true;

        // 1. Name Filter (Case-insensitive partial match)
        if (!string.IsNullOrEmpty(request.Name))
        {
            filter = filter.And(card => card.Name.Contains(request.Name));
        }

        // 2. Date of Birth (Exact match for the date part)
        if (request.DateOfBirth.HasValue)
        {
            // Note: Since DateOfBirth in the entity is DateTime, you compare the Date property.
            filter = filter.And(card => card.DateOfBirth.Date == request.DateOfBirth.Value.Date);
        }

        // 3. Phone Filter (Case-insensitive partial match)
        if (!string.IsNullOrEmpty(request.Phone))
        {
            filter = filter.And(card => card.Phone.Contains(request.Phone));
        }

        // 4. Gender Filter (Case-insensitive exact match)
        if (!string.IsNullOrEmpty(request.Gender))
        {
            filter = filter.And(card => card.Gender.Equals(request.Gender, StringComparison.CurrentCultureIgnoreCase));
        }

        // 5. Email Filter (Case-insensitive partial match)
        if (!string.IsNullOrEmpty(request.Email))
        {
            filter = filter.And(card => card.Email.Contains(request.Email));
        }

        PaginationResult<BusinessCard> result = await _businessCardRepository.GetAllAsync(request.PageNumber, request.Size, filter);

        return new GetAllBusinessCardsQueryResult(result);
    }
}
