using System.Security.Claims;

using ProgressSoft.Domain.Entities;
using ProgressSoft.Domain.Entities.Authentication;

namespace ProgressSoft.Domain.Interfaces.Application.Services;

public interface IJwtService
{
    Task<string> GenerateAccessTokenAsync(User user);
    RefreshToken CreateRefreshTokenEntity(
        User user,
        Guid tokenFamilyId);
    Task<ClaimsPrincipal> ValidateToken(string token);
}
