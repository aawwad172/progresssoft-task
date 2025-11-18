using ProgressSoft.Domain.DTOs;
using ProgressSoft.Domain.Interfaces.Infrastructure.IParsers;
using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

using SixLabors.ImageSharp; // Nuget: SixLabors.ImageSharp
using SixLabors.ImageSharp.PixelFormats;

using ZXing.ImageSharp;     // Nuget: ZXing.Net.Bindings.ImageSharp
namespace ProgressSoft.Infrastructure.Persistence.Parsers;

public class QrCodeParser : IQRCodeParser
{
    public async Task<IFileImportRepository.ImportResult> ParseAsync(Stream stream)
    {
        List<string> errors = [];
        List<BusinessCardCreateDto> records = [];

        try
        {
            // 1. Reset stream position
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);

            // 2. Load image from stream using ImageSharp (Linux/Docker safe)
            using var image = await Image.LoadAsync<Rgba32>(stream);

            // 3. Setup the Reader
            var reader = new BarcodeReader<Rgba32>();

            // 4. Decode
            var result = reader.Decode(image);

            if (result != null && !string.IsNullOrWhiteSpace(result.Text))
            {
                // 5. Convert the string content to DTO
                // We assume the QR content is a vCard string or a structured text
                var dto = ParseVCardTextToDto(result.Text);
                records.Add(dto);
            }
            else
            {
                errors.Add("No valid QR code could be detected in the uploaded image.");
            }
        }
        catch (Exception ex)
        {
            errors.Add($"Failed to read QR Code: {ex.Message}");
        }

        return new IFileImportRepository.ImportResult(records.AsReadOnly(), errors.AsReadOnly());
    }

    // --- Helper: Parse Raw Text to DTO ---
    private BusinessCardCreateDto ParseVCardTextToDto(string text)
    {
        BusinessCardCreateDto dto = new();

        // Split by new lines
        string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            // Get the part AFTER the colon
            // Example: "Name: John Doe" -> "John Doe"
            string value = line.Contains(':')
                ? line.Substring(line.IndexOf(':') + 1).Trim()
                : string.Empty;

            // 1. Name
            if (line.StartsWith("FN:", StringComparison.OrdinalIgnoreCase) ||
                line.StartsWith("Name:", StringComparison.OrdinalIgnoreCase))
            {
                dto.Name = value;
            }
            // 2. Email
            else if (line.StartsWith("EMAIL:", StringComparison.OrdinalIgnoreCase))
            {
                dto.Email = value;
            }
            // 3. Phone
            else if (line.StartsWith("TEL:", StringComparison.OrdinalIgnoreCase) ||
                     line.StartsWith("Phone:", StringComparison.OrdinalIgnoreCase))
            {
                dto.Phone = value;
            }
            // 4. Address
            else if (line.StartsWith("ADR:", StringComparison.OrdinalIgnoreCase) ||
                     line.StartsWith("Address:", StringComparison.OrdinalIgnoreCase))
            {
                dto.Address = value.Replace(";", " "); // Clean vCard semicolons if present
            }
            // 5. Gender (Direct String Assignment)
            else if (line.StartsWith("Gender:", StringComparison.OrdinalIgnoreCase) ||
                     line.StartsWith("X-GENDER:", StringComparison.OrdinalIgnoreCase))
            {
                // If the QR says "Gender: Male", this assigns "Male"
                // If the QR says "Gender: male", this assigns "male"
                dto.Gender = value;
            }
            else if (line.StartsWith("BDAY:", StringComparison.OrdinalIgnoreCase))
            {
                string dateString = value;

                // Try to parse it safely into a DateTime (or DateOnly if using .NET 6+)
                if (DateTime.TryParse(dateString, out DateTime dob))
                {
                    dto.DateOfBirth = dob;
                }
            }
        }

        return dto;
    }
}
