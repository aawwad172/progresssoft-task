using ProgressSoft.Domain.DTOs;

namespace ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

public interface IFileImportRepository
{
    /// <summary>
    /// Record to hold the results of a parsing operation, including successful DTOs and any parsing errors.
    /// This is the standardized output for all ICsvParser, IXmlParser, etc., implementations.
    /// </summary>
    public record ImportResult(
        IReadOnlyList<BusinessCardCreateDto> BusinessCards,
        IReadOnlyList<string> Errors);

    /// <summary>
    /// Reads a file stream, determines file type, and parses the contents into DTOs.
    /// </summary>
    Task<ImportResult> ImportAsync(string fileName, Stream stream);
}
