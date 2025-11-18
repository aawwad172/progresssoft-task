using System.Globalization;
using System.Text;

using CsvHelper;
using CsvHelper.Configuration;

using ProgressSoft.Domain.DTOs;
using ProgressSoft.Domain.Enums;
using ProgressSoft.Domain.Interfaces.Infrastructure.IExporters;

namespace ProgressSoft.Infrastructure.Persistence.Exporters;

/// <summary>
/// Implements the ICsvExporter interface using the CsvHelper library.
/// </summary>
public class CsvExporter : IFileExporter
{
    public FileFormatEnum Format => FileFormatEnum.Csv;
    public string ContentType => "text/csv";
    public string FileExtension => ".csv";

    public async Task<byte[]> ExportAsync<T>(IEnumerable<T> data)
    {
        // Pass the specific logic: "Write all records"
        return await GenerateCsvBytesAsync<T>(async csv =>
        {
            await csv.WriteRecordsAsync(data);
        });
    }

    public async Task<byte[]> ExportAsync<T>(T data)
    {
        // Pass the specific logic: "Write Header -> Next Line -> Write One Record"
        return await GenerateCsvBytesAsync<T>(async csv =>
        {
            // When writing a single record manually, we must trigger the header explicitly
            csv.WriteHeader<T>();
            await csv.NextRecordAsync();
            csv.WriteRecord(data);
        });
    }

    // --- PRIVATE HELPER ---
    // This method handles the "boilerplate": Streams, Writers, Configuration, and Mapping.
    // It accepts an action (Func) to perform the actual writing operation.
    private async Task<byte[]> GenerateCsvBytesAsync<T>(Func<CsvWriter, Task> writeAction)
    {
        using MemoryStream memoryStream = new();
        using StreamWriter writer = new(memoryStream, Encoding.UTF8);
        using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);

        // Shared Logic: Register Map if applicable
        if (typeof(T) == typeof(BusinessCardCreateDto))
        {
            csv.Context.RegisterClassMap<BusinessCardMap>();
        }

        // Execute the specific writing logic passed from the public methods
        await writeAction(csv);

        // Finalize stream
        await writer.FlushAsync();
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
