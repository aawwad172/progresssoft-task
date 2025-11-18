using System.Xml.Serialization;

using ProgressSoft.Domain.DTOs;
using ProgressSoft.Domain.Interfaces.Infrastructure.IExporters;
using ProgressSoft.Infrastructure.Persistence.Parsers;

namespace ProgressSoft.Infrastructure.Persistence.Exporters;

/// <summary>
/// Implements the IXmlExporter interface using the .NET XmlSerializer.
/// </summary>
public class XmlExporter : IXmlExporter
{
    public async Task<byte[]> ExportAsync(IEnumerable<BusinessCardCreateDto> cards)
    {
        // 1. Create the collection object that matches the XML structure
        // We use the XmlBusinessCardCollection DTO from your Infrastructure layer.
        XmlBusinessCardCollection collection = new()
        {
            Cards = cards.ToList()
        };

        XmlSerializer serializer = new(typeof(XmlBusinessCardCollection));

        using MemoryStream memoryStream = new();

        // 2. Serialize the collection object into the stream.
        // XmlSerializer does not have a built-in async method,
        // so we wrap the synchronous call in Task.Run to keep the method async.
        await Task.Run(() =>
        {
            serializer.Serialize(memoryStream, collection);
        });

        // 3. Return the bytes from the stream
        return memoryStream.ToArray();
    }
}
