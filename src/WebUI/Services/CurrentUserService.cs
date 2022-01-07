using System.Security.Claims;
using DeliveryWebApp.Application.Common.Interfaces;

namespace WebUI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly HttpContextAccessor _httpContextAccessor;

    public CurrentUserService(HttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
}