using ProgressSoft.Domain.Interfaces.Application.Services;

namespace ProgressSoft.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    public Guid UserId { get; set; }
}
