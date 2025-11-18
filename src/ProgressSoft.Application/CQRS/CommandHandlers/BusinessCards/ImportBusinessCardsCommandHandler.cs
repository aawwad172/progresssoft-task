using Microsoft.Extensions.Logging;

using ProgressSoft.Application.CQRS.Commands.BusinessCards;
using ProgressSoft.Domain.Entities;
using ProgressSoft.Domain.Interfaces.Application.Services;
using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

namespace ProgressSoft.Application.CQRS.CommandHandlers.BusinessCards;

public class ImportBusinessCardsCommandHandler(
    ICurrentUserService currentUserService,
    ILogger<ImportBusinessCardsCommandHandler> logger,
    IUnitOfWork unitOfWork,
    IRepository<BusinessCard> businessCardRepository)
    : BaseHandler<ImportBusinessCardsCommand, ImportBusinessCardsCommandResult>(currentUserService, logger, unitOfWork)
{
    private readonly IRepository<BusinessCard> _businessCardRepository = businessCardRepository;
    public override async Task<ImportBusinessCardsCommandResult> Handle(ImportBusinessCardsCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();
        if (request.Cards is null || !request.Cards.Any())
            throw new ArgumentNullException("Cannot insert empty list of business cards");

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            DateTime creationTime = DateTime.UtcNow;

            List<BusinessCard> businessCardEntities = request.Cards.Select(dto => new BusinessCard
            {
                Id = Guid.CreateVersion7(), // ⬅️ Generates a unique, time-ordered ID for each card
                Name = dto.Name,
                Gender = dto.Gender,
                DateOfBirth = DateTime.SpecifyKind(dto.DateOfBirth, DateTimeKind.Utc),
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                PhotoBase64 = dto.PhotoBase64,
                CreatedAt = creationTime // Sets the creation time consistently for the whole batch
            }).ToList();

            await _businessCardRepository.AddBulkAsync(businessCardEntities);

            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync();

            return new ImportBusinessCardsCommandResult(businessCardEntities.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding business cards in bulk.");
            await _unitOfWork.RollbackAsync();
            throw; // Re-throw the exception for upper layers to handle
        }
    }
}
