using LiterasData.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LiterasWebAPI.Config;
public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        Log.Error($"!--- {context.Exception.Message} ---! " +
                  $"{Environment.NewLine} {Environment.NewLine} " +
                  $"{context.Exception.StackTrace} " +
                  $"{Environment.NewLine} {Environment.NewLine}");
        context.ExceptionHandled = true;
        context.Result = new ContentResult
        {
            StatusCode = GetExceptionStatusCode(context.Exception),
            Content = context.Exception.Message
        };
    }

    private static int GetExceptionStatusCode(Exception ex)
    {
        return ex switch
        {
            NotModifiedException => StatusCodes.Status304NotModified,
            NotFoundException => StatusCodes.Status400BadRequest,
            ForbiddenException => StatusCodes.Status403Forbidden,
            RaceException or DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
            GeneralException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError,
        };
    }
}
