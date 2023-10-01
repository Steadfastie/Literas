using System.Security.Claims;
using LiterasCore.Abstractions;
using LiterasData.Exceptions;
using Serilog.Context;

namespace LiterasWebAPI.Middleware;

public class UserInfoMiddleware : IMiddleware
{
    private readonly IIdentityService _identityService;

    public UserInfoMiddleware(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim) || !userIdClaim.StartsWith("auth0|"))
        {
            throw new GeneralException("Service could not recognize user");
        }

        _identityService.UserId = userIdClaim;

        var userId = $"**{userIdClaim[^4..]}";

        using (LogContext.PushProperty("User", userId))
        {
            await next(context);
        }
    }
}
