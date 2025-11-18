using ProgressSoft.Domain.DTOs;

namespace ProgressSoft.Domain.Interfaces.Infrastructure.IExporters;

/// <summary>
/// Base interface for all file exporters.
/// </summary>
public interface IFileExporter
{
    /// <summary>
    /// Serializes a list of DTOs into a byte array representing a file.
    /// </summary>
    /// <param name="cards">The list of DTOs to serialize.</param>
    /// <returns>A byte array of the generated file.</returns>
    Task<byte[]> ExportAsync(IEnumerable<BusinessCardCreateDto> cards);
}
