using System.Globalization;
using System.Text;

using CsvHelper;
using CsvHelper.Configuration;

using ProgressSoft.Domain.DTOs;
using ProgressSoft.Domain.Interfaces.Infrastructure.IExporters;

namespace ProgressSoft.Infrastructure.Persistence.Exporters;

/// <summary>
/// Implements the ICsvExporter interface using the CsvHelper library.
/// </summary>
public class CsvExporter : ICsvExporter
{
    public async Task<byte[]> ExportAsync(IEnumerable<BusinessCardCreateDto> cards)
    {
        // We write to a MemoryStream instead of a physical file
        using MemoryStream memoryStream = new();

        // Use a StreamWriter to write text (CSV) into the stream
        // We use UTF-8 to support international characters
        using StreamWriter writer = new(memoryStream, Encoding.UTF8);

        // CsvWriter handles the CSV formatting
        using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);

        // Register the map. This ensures the headers are written correctly
        // and in the order we specify.
        csv.Context.RegisterClassMap<BusinessCardMap>();

        // Write all records from the DTO list into the stream
        await csv.WriteRecordsAsync(cards);

        // Flush the writer to ensure all data is pushed to the MemoryStream
        await writer.FlushAsync();

        // Return the complete byte array from the stream
        return memoryStream.ToArray();
    }
}

/// <summary>
/// Defines the column headers and order for the exported CSV file.
/// We can reuse the same map definitions from the parser.
/// </summary>
internal sealed class BusinessCardMap : ClassMap<BusinessCardCreateDto>
{
    public BusinessCardMap()
    {
        Map(m => m.Name).Name("Name");
        Map(m => m.Gender).Name("Gender");
        Map(m => m.DateOfBirth).Name("DateOfBirth");
        Map(m => m.Email).Name("Email");
        Map(m => m.Phone).Name("Phone");
        Map(m => m.Address).Name("Address");
        Map(m => m.PhotoBase64).Name("PhotoBase64");
    }
}
