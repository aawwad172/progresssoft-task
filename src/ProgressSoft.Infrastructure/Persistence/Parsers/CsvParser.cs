using System.Globalization;
using System.Text;

using CsvHelper;
using CsvHelper.Configuration;

using ProgressSoft.Domain.DTOs;
using ProgressSoft.Domain.Interfaces.Infrastructure.IParsers;
using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

namespace ProgressSoft.Infrastructure.Persistence.Parsers;

public class CsvParser : ICsvParser
{
    // Note: We use the standardized ImportResult wrapper (which we assume is now generic)
    public async Task<IFileImportRepository.ImportResult> ParseAsync(Stream stream)
    {
        List<BusinessCardCreateDto> records = [];
        List<string> errors = [];

        CsvConfiguration config = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            DetectDelimiter = true,
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
            // Use property normalization for mapping flexibility
            PrepareHeaderForMatch = args => args.Header.ToLowerInvariant().Replace(" ", "").Replace("_", "")
        };

        try
        {
            using StreamReader reader = new(stream, Encoding.UTF8); // Use UTF8 encoding
            using CsvReader csv = new(reader, config);

            await foreach (var record in csv.GetRecordsAsync<BusinessCardCreateDto>())
            {
                records.Add(record);
            }
        }
        catch (Exception ex)
        {
            errors.Add($"An unexpected error occurred during CSV parsing: {ex.Message}");
        }

        // 3. Return generic ImportResult<T>
        return new IFileImportRepository.ImportResult(records.AsReadOnly(), errors.AsReadOnly());
    }
}
