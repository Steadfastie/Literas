﻿using System.Security.Claims;
using Serilog.Context;

namespace LiterasWebAPI.Middleware;

public class LoggerEnricherMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var userId = context.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        userId = userId != null ? $"**{userId[^4..]}" : "UnAuthUser";

        using (LogContext.PushProperty("User", userId))
        {
            await next(context);
        }
    }
}