using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ProgressSoft.Application.Services;
using ProgressSoft.Application.Utilities.Extensions;
using ProgressSoft.Domain.Interfaces.Application.Services;
using ProgressSoft.Domain.Interfaces.Infrastructure.IExporters;
using ProgressSoft.Domain.Interfaces.Infrastructure.IParsers;
using ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;
using ProgressSoft.Infrastructure.Persistence;
using ProgressSoft.Infrastructure.Persistence.Exporters;
using ProgressSoft.Infrastructure.Persistence.Parsers;
using ProgressSoft.Infrastructure.Persistence.Repositories;

namespace ProgressSoft.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetRequiredSetting("ConnectionStrings:DbConnectionString");

        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });

        // Add your repositories like this here
        // services.AddScoped<IRepository, Repository>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<IFileImportRepository, FileImportRepository>();

        services.AddScoped<ICsvParser, CsvParser>();
        services.AddScoped<IXmlParser, XmlParser>();
        services.AddScoped<IQRCodeParser, QrCodeParser>();

        // Step 1: Register CSV implementation as IFileExporter
        services.AddScoped<IFileExporter, CsvExporter>();
        // Step 2: Register XML implementation as IFileExporter
        services.AddScoped<IFileExporter, XmlExporter>();

        services.AddScoped<IExporterFactory, ExporterFactory>();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddLogging();

        return services;
    }
}
