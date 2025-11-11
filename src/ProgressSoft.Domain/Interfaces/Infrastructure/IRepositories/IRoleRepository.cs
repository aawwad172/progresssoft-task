using ProgressSoft.Domain.Entities.Authentication;

namespace ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

public interface IRoleRepository
{
    Task<Role?> GetRoleByNameAsync(string name);
}
