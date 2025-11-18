using Microsoft.Extensions.Logging;

using ProgressSoft.Application.CQRS.Queries;
using ProgressSoft.Domain.DTOs;
using ProgressSoft.Domain.Entities;
using ProgressSoft.Domain.Exceptions;
using ProgressSoft.Domain.Interfaces.Application.Services;
using ProgressSoft.Domain.Interfaces.Infrastructure.IExporters;
using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

namespace ProgressSoft.Application.CQRS.QueryHandlers;

public class ExportBusinessCardQueryHandler(
    ICurrentUserService currentUserService,
    ILogger<ExportBusinessCardQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IExporterFactory exporterFactory,
    IRepository<BusinessCard> businessCardRepository)
    : BaseHandler<ExportBusinessCardQuery, ExportBusinessCardQueryResult>(currentUserService, logger, unitOfWork)
{
    private readonly IRepository<BusinessCard> _businessCardRepository = businessCardRepository;
    private readonly IExporterFactory _exporterFactory = exporterFactory;
    public override async Task<ExportBusinessCardQueryResult> Handle(ExportBusinessCardQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch ALL data (no pagination, no filter)
        var card = await _businessCardRepository.GetByIdAsync(request.Id);

        if (card is null)
        {
            throw new NotFoundException($"No business card with id {request.Id} found to export.");
        }

        // 2. Map Entities to DTOs
        BusinessCardCreateDto cardDto = new()
        {
            Name = card.Name,
            Gender = card.Gender,
            DateOfBirth = card.DateOfBirth,
            Email = card.Email,
            Phone = card.Phone,
            Address = card.Address,
            PhotoBase64 = card.PhotoBase64
        };

        IFileExporter exporter = _exporterFactory.GetExporter(request.Format);

        // 3. Generate Bytes
        byte[] fileContents = await exporter.ExportAsync(cardDto);

        string contentType = exporter.ContentType;
        string fileName = $"BusinessCard_{card.Id}{exporter.FileExtension}";

        // 4. Return the file
        return new ExportBusinessCardQueryResult(fileContents, contentType, fileName);
    }
}
