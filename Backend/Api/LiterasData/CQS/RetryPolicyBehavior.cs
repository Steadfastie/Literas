using System.Reflection;
using LiterasData.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;

namespace LiterasData.CQS;

/// <summary>
///     Wraps request handler execution of requests decorated with the <see cref="RetryPolicyAttribute" />
///     inside a policy to handle transient failures and retry the execution.
/// </summary>
public class RetryPolicyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<RetryPolicyBehavior<TRequest, TResponse>> _logger;

    public RetryPolicyBehavior(ILogger<RetryPolicyBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var retryAttr = typeof(TRequest).GetCustomAttribute<RetryPolicyAttribute>();
        if (retryAttr == null)
        {
            return await next();
        }

        return await Policy
            .Handle<RaceException>()
            .Or<DbUpdateConcurrencyException>()
            .WaitAndRetryAsync(3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, _, _) =>
                    _logger.LogWarning(ex, "Race condition appeared"))
            .ExecuteAsync(async () => await next());
    }
}
