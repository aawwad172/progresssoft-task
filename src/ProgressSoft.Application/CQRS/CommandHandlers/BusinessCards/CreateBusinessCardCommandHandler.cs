using Microsoft.Extensions.Logging;

using ProgressSoft.Application.CQRS.Commands.BusinessCards;
using ProgressSoft.Domain.Entities;
using ProgressSoft.Domain.Interfaces.Application.Services;
using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

namespace ProgressSoft.Application.CQRS.CommandHandlers.BusinessCards;

public class CreateBusinessCardCommandHandler(
    ICurrentUserService currentUserService,
    ILogger<CreateBusinessCardCommandHandler> logger,
    IUnitOfWork unitOfWork,
    IRepository<BusinessCard> businessCardRepository)
    : BaseHandler<CreateBusinessCardCommand, CreateBusinessCardCommandResult>(currentUserService, logger, unitOfWork)
{
    private readonly IRepository<BusinessCard> _businessCardRepository = businessCardRepository;

    public override async Task<CreateBusinessCardCommandResult> Handle(
        CreateBusinessCardCommand request,
        CancellationToken cancellationToken)
    {
        Guid id = Guid.CreateVersion7();
        DateTime now = DateTime.UtcNow;

        BusinessCard card = new()
        {
            Id = id,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Gender = request.Gender,
            DateOfBirth = request.DateOfBirth,
            PhotoBase64 = request.PhotoBase64,
            Address = request.Address,
            CreatedAt = now,
            CreatedBy = Guid.Empty,
            UpdatedAt = null,
            UpdatedBy = Guid.Empty,
            IsDeleted = false
        };

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            BusinessCard newCard = await _businessCardRepository.AddAsync(card);

            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync();

            return new CreateBusinessCardCommandResult(newCard);
        }
        catch
        {
            _logger.LogError("Couldn't create business card");
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
