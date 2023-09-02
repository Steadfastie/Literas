using System.Security.Claims;
using LiterasCore.Abstractions;
using LiterasData.Exceptions;

namespace LiterasWebAPI.Middleware;

public class UserInfoExtractorMiddleware : IMiddleware
{
    private readonly IIdentityService _identityService;

    public UserInfoExtractorMiddleware(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new GeneralException("Service could not recognize user");
        }

        _identityService.UserId = userId;
        await next(context);
    }
}
