using ProgressSoft.Application.Services;
using ProgressSoft.Domain.Interfaces.Application.Services;

using MapsterMapper;

using Microsoft.Extensions.DependencyInjection;

namespace ProgressSoft.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IMapper, Mapper>();

        return services;
    }
}
