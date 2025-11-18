using ProgressSoft.Domain.Enums;

namespace ProgressSoft.Domain.Interfaces.Infrastructure.IExporters;

/// <summary>
/// Base interface for all file exporters.
/// </summary>
public interface IFileExporter
{
    // This property helps us choose the right implementation dynamically
    FileFormatEnum Format { get; } // e.g., "csv", "xml"
    string ContentType { get; }   // e.g. "text/csv"
    string FileExtension { get; }

    /// <summary>
    /// Serializes a list of DTOs into a byte array representing a file.
    /// </summary>
    /// <param name="cards">The list of DTOs to serialize.</param>
    /// <returns>A byte array of the generated file.</returns>
    Task<byte[]> ExportAsync<T>(IEnumerable<T> data);

    /// <summary>
    /// Serializes a  DTO into a byte array representing a file.
    /// </summary>
    /// <param name="card">The DTO to serialize.</param>
    /// <returns>A byte array of the generated file.</returns>
    Task<byte[]> ExportAsync<T>(T data);
} // e.g. ".csv"
