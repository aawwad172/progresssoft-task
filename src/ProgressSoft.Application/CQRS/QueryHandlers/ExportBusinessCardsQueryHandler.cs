using Microsoft.Extensions.Logging;

using ProgressSoft.Application.CQRS.Queries;
using ProgressSoft.Domain.DTOs;
using ProgressSoft.Domain.Entities;
using ProgressSoft.Domain.Enums;
using ProgressSoft.Domain.Interfaces.Application.Services;
using ProgressSoft.Domain.Interfaces.Infrastructure.IExporters;
using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

namespace ProgressSoft.Application.CQRS.QueryHandlers;

public class ExportBusinessCardsQueryHandler(
    ICurrentUserService currentUserService,
    ILogger<ExportBusinessCardsQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IRepository<BusinessCard> businessCardRepository,
    ICsvExporter csvExporter,
    IXmlExporter xmlExporter)
    : BaseHandler<ExportBusinessCardsQuery, ExportBusinessCardsResult>(currentUserService, logger, unitOfWork)
{
    private readonly IRepository<BusinessCard> _businessCardRepository = businessCardRepository;
    private readonly ICsvExporter _csvExporter = csvExporter;
    private readonly IXmlExporter _xmlExporter = xmlExporter;

    public override async Task<ExportBusinessCardsResult> Handle(ExportBusinessCardsQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch ALL data (no pagination, no filter)
        PaginationResult<BusinessCard> paginationResult = await _businessCardRepository.GetAllAsync(
            pageNumber: null,
            pageSize: null,
            filter: null);

        var cards = paginationResult.Page;

        if (cards is null || !cards.Any())
        {
            throw new Exception("No business cards found to export.");
        }

        // 2. Map Entities to DTOs
        IEnumerable<BusinessCardCreateDto> cardDtos = cards.Select(card => new BusinessCardCreateDto
        {
            Name = card.Name,
            Gender = card.Gender,
            DateOfBirth = card.DateOfBirth,
            Email = card.Email,
            Phone = card.Phone,
            Address = card.Address,
            PhotoBase64 = card.PhotoBase64
        });

        // 3. Delegate to the correct exporter
        byte[] fileContents;
        string contentType;
        string fileName;
        FileFormatEnum format = request.Format;

        if (format == FileFormatEnum.Csv)
        {
            fileContents = await _csvExporter.ExportAsync(cardDtos);
            contentType = "text/csv";
            fileName = $"BusinessCards_Export_{DateTime.UtcNow:yyyyMMdd}.csv";
        }
        else if (format == FileFormatEnum.Xml)
        {
            fileContents = await _xmlExporter.ExportAsync(cardDtos);
            contentType = "application/xml";
            fileName = $"BusinessCards_Export_{DateTime.UtcNow:yyyyMMdd}.xml";
        }
        else
        {
            throw new NotSupportedException($"Export format '{request.Format}' is not supported. Please use 'csv' or 'xml'.");
        }

        // 4. Return the file
        return new ExportBusinessCardsResult(fileContents, contentType, fileName);
    }
}
