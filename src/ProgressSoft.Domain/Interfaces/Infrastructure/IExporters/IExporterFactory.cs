using ProgressSoft.Domain.Enums;

namespace ProgressSoft.Domain.Interfaces.Infrastructure.IExporters;

public interface IExporterFactory
{
    IFileExporter GetExporter(FileFormatEnum format);
}
