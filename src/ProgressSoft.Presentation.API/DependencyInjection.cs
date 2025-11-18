using System.Reflection;
using System.Text;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using ProgressSoft.Application.CQRS.CommandHandlers.BusinessCards;
using ProgressSoft.Application.Utilities.Extensions;
using ProgressSoft.Domain.Entities.Authentication;
using ProgressSoft.Domain.Enums;
using ProgressSoft.Presentation.API.Validators.Commands.Authentication;

namespace ProgressSoft.Presentation.API;

public static class DependencyInjection
{
    /// <summary>
    /// Registers Presentation layer services such as controllers, MediatR, FluentValidation, and any pipeline behaviors.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        // --- 2. FIX FOR MEDIATR ---
        // Get the assembly where your HANDLERS are (the Application layer)
        var applicationAssembly = typeof(ImportBusinessCardsCommandHandler).Assembly;

        // This now correctly scans your Application assembly for handlers
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Register FluentValidation validators found in the current assembly.
        // services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
        // services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
        // services.AddValidatorsFromAssemblyContaining<RefreshTokenCommandValidator>();
        // services.AddValidatorsFromAssemblyContaining<LogoutCommandValidator>();

        // services.AddValidatorsFromAssemblyContaining<GetAllBusinessCardsQueryValidator>();
        // services.AddValidatorsFromAssemblyContaining<GetBusinessCardByIdQueryValidator>();
        // services.AddValidatorsFromAssemblyContaining<CreateBusinessCardCommandValidator>();
        // services.AddValidatorsFromAssemblyContaining<DeleteBusinessCardCommandValidator>();
        // services.AddValidatorsFromAssemblyContaining<ImportBusinessCardsCommandValidator>();

        // --- 3. FIX FOR VALIDATORS ---
        // Get the assembly where your VALIDATORS are (the Presentation.API layer)
        var presentationAssembly = typeof(RegisterUserCommandValidator).Assembly;

        // This scans the Presentation.API assembly ONCE and registers all validators
        services.AddValidatorsFromAssembly(presentationAssembly);


        services.AddHttpContextAccessor();

        services.AddLogging(configure =>
        {
            configure.ClearProviders();
            configure.AddConsole();
            configure.AddDebug();
        });

        // Optionally, register pipeline behaviors (for example, a transactional behavior).
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        // Configure JWT authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration.GetRequiredSetting("Jwt:Issuer"),
                ValidAudience = configuration.GetRequiredSetting("Jwt:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetRequiredSetting("Jwt:JwtSecretKey")))
            };
        });

        services.AddAuthorization(options =>
        {
            // 1. Policy for creating a post (e.g., for /posts endpoint)
            options.AddPolicy("PostApprove", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(CustomClaims.Permission, PermissionConstants.PostApprove);
            });

            // 2. Policy for managing users (e.g., for /users/ endpoint)
            options.AddPolicy("UserRead", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(CustomClaims.Permission, PermissionConstants.UserRead);
            });
        });

        return services;
    }
}
