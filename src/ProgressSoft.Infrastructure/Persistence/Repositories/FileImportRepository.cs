using ProgressSoft.Domain.Exceptions;
using ProgressSoft.Domain.Interfaces.Infrastructure.IParsers;
using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

namespace ProgressSoft.Infrastructure.Persistence.Repositories;

public class FileImportRepository(
    ICsvParser csvParser,
    IXmlParser xmlParser,
    IQRCodeParser qrCodeParser) : IFileImportRepository
{
    private readonly ICsvParser _csvParser = csvParser;
    private readonly IXmlParser _xmlParser = xmlParser;
    private readonly IQRCodeParser _qrCodeParser = qrCodeParser;

    public async Task<IFileImportRepository.ImportResult> ImportAsync(string fileName, Stream stream)
    {
        if (stream == null || stream.Length == 0)
        {
            throw new FileStreamEmptyException("The file stream is empty.");
        }

        string extension = NormalizeExtension(fileName);

        // 1. Determine the parser based on file extension
        IParser parser = extension switch
        {
            ".csv" => _csvParser,
            ".xml" => _xmlParser,
            // QR codes are typically embedded in image files (png/jpg)
            ".png" or ".jpg" or ".jpeg" => _qrCodeParser,
            _ => throw new NotSupportedException($"File type '{extension}' is not supported for business card import.")
        };

        // 2. Delegate the parsing job
        // Note: The specialized parser handles the stream reading and DTO mapping.
        IFileImportRepository.ImportResult result = await parser.ParseAsync(stream);

        return result;
    }

    private static string NormalizeExtension(string fileName)
        => Path.GetExtension(fileName).ToLowerInvariant();
}
