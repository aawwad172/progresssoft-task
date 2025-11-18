using Microsoft.Extensions.Logging;

using ProgressSoft.Application.CQRS.Commands.BusinessCards;
using ProgressSoft.Domain.Entities;
using ProgressSoft.Domain.Exceptions;
using ProgressSoft.Domain.Interfaces.Application.Services;
using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

namespace ProgressSoft.Application.CQRS.CommandHandlers.BusinessCards;

public class DeleteBusinessCardCommandHandler(
    ICurrentUserService currentUserService,
    ILogger<DeleteBusinessCardCommandHandler> logger,
    IUnitOfWork unitOfWork,
    IRepository<BusinessCard> businessCardRepository)
    : BaseHandler<DeleteBusinessCardCommand, DeleteBusinessCardCommandResult>(currentUserService, logger, unitOfWork)
{
    private readonly IRepository<BusinessCard> _businessCardRepository = businessCardRepository;
    public override async Task<DeleteBusinessCardCommandResult> Handle(DeleteBusinessCardCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            BusinessCard? card = await _businessCardRepository.GetByIdAsync(request.Id);
            if (card is null)
                throw new NotFoundException($"Business card with Id: {request.Id} does not found");

            await _businessCardRepository.DeleteAsync(card.Id);

            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync();

            return new DeleteBusinessCardCommandResult(true);
        }
        catch
        {
            _logger.LogError($"Error while trying to delete card with Id: {request.Id}");
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
