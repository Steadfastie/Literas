using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
            Content = context.Exception.ToString()
        };
    }

    private static int GetExceptionStatusCode(Exception ex)
    {
        return ex.GetType() switch
        {
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
