using ProgressSoft.Domain.Enums;
using ProgressSoft.Domain.Interfaces.Infrastructure.IExporters;

namespace ProgressSoft.Infrastructure.Persistence.Exporters;

public class ExporterFactory(IEnumerable<IFileExporter> exporters) : IExporterFactory
{
    private readonly IEnumerable<IFileExporter> _exporters = exporters;

    public IFileExporter GetExporter(FileFormatEnum format)
    {
        // 2. Find the implementation where the .Format property matches
        IFileExporter? exporter = _exporters.FirstOrDefault(x => x.Format == format);

        if (exporter == null)
        {
            throw new NotSupportedException($"No exporter implementation found for '{format}'.");
        }

        return exporter;
    }
}
