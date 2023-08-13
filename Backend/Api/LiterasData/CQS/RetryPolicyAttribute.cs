using MediatR;

namespace LiterasData.CQS;

/// <summary>
/// Applies a retry policy on the MediatR request.
/// Apply this attribute to the MediatR <see cref="IRequest{TResponse}"/> class (not on the handler).
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class RetryPolicyAttribute : Attribute
{
    private int _retryCount = 3;

    /// <summary>
    /// Gets or sets the amount of times to retry the execution.
    /// Defaults to 3 times.
    /// </summary>
    public int RetryCount
    {
        get => _retryCount;
        set
        {
            if (value < 1)
            {
                throw new ArgumentException("Retry count must be higher than 1.", nameof(value));
            }

            _retryCount = value;
        }
    }
}
