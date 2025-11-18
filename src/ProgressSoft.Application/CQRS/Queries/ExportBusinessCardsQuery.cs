using MediatR;

using ProgressSoft.Domain.Enums;

namespace ProgressSoft.Application.CQRS.Queries;

/// <summary>
/// Query to export ALL business cards to a file (CSV or XML).
/// </summary>
public sealed record ExportBusinessCardsQuery(
    // Defines the desired file format ("csv" or "xml")
    FileFormatEnum Format
) : IRequest<ExportBusinessCardsResult>;

/// <summary>
/// Holds the result of a successful file export.
/// </summary>
public sealed record ExportBusinessCardsResult(
    byte[] FileContents,    // The raw data of the file
    string ContentType,     // e.g., "text/csv" or "application/xml"
    string FileName         // e.g., "BusinessCards.csv"
);
