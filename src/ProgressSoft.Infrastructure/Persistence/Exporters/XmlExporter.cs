using System.Text;
using System.Xml;
using System.Xml.Serialization;

using ProgressSoft.Domain.DTOs;
using ProgressSoft.Domain.Enums;
using ProgressSoft.Domain.Interfaces.Infrastructure.IExporters;

namespace ProgressSoft.Infrastructure.Persistence.Exporters;

/// <summary>
/// Implements the IXmlExporter interface using the .NET XmlSerializer.
/// </summary>
public class XmlExporter : IFileExporter
{
    public FileFormatEnum Format => FileFormatEnum.Xml;
    public string ContentType => "application/xml";
    public string FileExtension => ".xml";

    public async Task<byte[]> ExportAsync<T>(IEnumerable<T> data)
    {
        // Dynamic Root Name Logic:
        // If it is a BusinessCard, use "BusinessCards". Otherwise, default to "ArrayOfData".
        string rootName = typeof(T) == typeof(BusinessCardCreateDto)
            ? "BusinessCards"
            : "Data";

        // We convert to List because XmlSerializer handles List<T> better than IEnumerable<T>
        return await Task.Run(() => SerializeToXml(data.ToList(), rootName));
    }

    public async Task<byte[]> ExportAsync<T>(T data)
    {
        // Dynamic Root Name Logic:
        // If it is a BusinessCard, use "BusinessCard". Otherwise, default to "Data".
        string rootName = typeof(T) == typeof(BusinessCardCreateDto)
            ? "BusinessCard"
            : "Data";

        return await Task.Run(() => SerializeToXml(data, rootName));
    }

    // --- PRIVATE GENERIC HELPER ---
    private byte[] SerializeToXml<TInput>(TInput data, string rootName)
    {
        using MemoryStream memoryStream = new();

        // Use XmlTextWriter to ensure UTF8 and pretty formatting (Indented)
        using XmlTextWriter xmlWriter = new(memoryStream, Encoding.UTF8);
        xmlWriter.Formatting = Formatting.Indented;

        // Define the Root Attribute dynamically based on the logic above
        XmlRootAttribute root = new(rootName);

        // Create the serializer for the specific type TInput (which could be List<T> or T)
        XmlSerializer serializer = new(typeof(TInput), root);

        serializer.Serialize(xmlWriter, data);

        return memoryStream.ToArray();
    }
}
