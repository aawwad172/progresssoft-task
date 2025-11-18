using System.Xml;
using System.Xml.Serialization;

using ProgressSoft.Domain.DTOs;
using ProgressSoft.Domain.Interfaces.Infrastructure.IParsers;
using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

namespace ProgressSoft.Infrastructure.Persistence.Parsers;

public class XmlParser : IXmlParser
{
    // Method signature is async and returns the standardized ImportResult
    public async Task<IFileImportRepository.ImportResult> ParseAsync(Stream stream)
    {
        List<string> errors = [];
        IReadOnlyList<BusinessCardCreateDto> successfulRecords = new List<BusinessCardCreateDto>();

        try
        {
            // Ensure stream position is at the beginning
            stream.Seek(0, SeekOrigin.Begin);

            // Use the CONCRETE wrapper class for serialization
            var serializer = new XmlSerializer(typeof(XmlBusinessCardCollection));

            using var reader = XmlReader.Create(stream, new XmlReaderSettings { Async = true });

            if (serializer.CanDeserialize(reader))
            {
                // Cast to the CONCRETE wrapper
                var collection = (XmlBusinessCardCollection?)serializer.Deserialize(reader);

                if (collection?.Cards != null && collection.Cards.Any())
                {
                    // Assign the list of DTOs
                    successfulRecords = collection.Cards.AsReadOnly();
                }
                else
                {
                    errors.Add("XML file was parsed successfully but contained no 'Card' records or the structure was empty.");
                }
            }
            else
            {
                errors.Add("XML root element does not match expected structure (<BusinessCards>) or the file is invalid.");
            }
        }
        catch (InvalidOperationException ex) when (ex.InnerException is XmlException || ex.InnerException is FormatException)
        {
            errors.Add($"XML formatting error detected: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            errors.Add($"An unexpected error occurred during XML parsing: {ex.Message}");
        }

        // Return the standardized ImportResult record (wrapping the successful records and errors)
        return new IFileImportRepository.ImportResult(
            successfulRecords,
            errors.AsReadOnly()
        );
    }
}
/// <summary>
/// A helper class defining the concrete structure for the XML file upload.
/// </summary>
[XmlRoot("BusinessCards")] // Defines the required root element name
public class XmlBusinessCardCollection
{
    // Defines the name of the repeating child element (each business card)
    [XmlElement("Card")]
    public List<BusinessCardCreateDto> Cards { get; set; } = new List<BusinessCardCreateDto>();
}