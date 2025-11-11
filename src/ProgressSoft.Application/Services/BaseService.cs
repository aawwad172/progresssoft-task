using Microsoft.Extensions.Logging;

namespace ProgressSoft.Application.Services;

public abstract class BaseService<T>
{
    protected readonly ILogger<T> _logger;

    protected BaseService(ILogger<T> logger)
    {
        _logger = logger;
    }
}
