using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

namespace ProgressSoft.Domain.Interfaces.Infrastructure.IParsers;

/// <summary>
/// Base interface for all business card file parsers.
/// Ensures all parsers return a standardized ImportResult.
/// </summary>
public interface IParser
{
    Task<IFileImportRepository.ImportResult> ParseAsync(Stream stream);
}
