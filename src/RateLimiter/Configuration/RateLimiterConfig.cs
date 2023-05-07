namespace RateLimiter.Configuration;

/// <summary>
/// Configuration for <see cref="T:RateLimiter.RateLimiter" />
/// </summary>
public class RateLimiterConfig
{
    /// <summary>
    /// Includes rate limiter functionality
    /// </summary>
    internal bool RequestLimiterEnabled { get; }
    /// <summary>
    /// Default time frame on the number of requests for all endpoints
    /// </summary>
    internal int DefaultRequestLimitMs { get; }
    /// <summary>
    /// Limit on the consecutive number of requests for all endpoints
    /// </summary>
    internal int DefaultRequestLimitCount { get; }
    /// <summary>
    /// Limit list for specific endpoint
    /// </summary>
    internal List<EndpointLimiterConfig>? EndpointLimits { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="T:RateLimiter.Configuration.RateLimiterConfig" />.
    /// </summary>
    /// <param name="requestLimiterEnabled">Includes rate limiter functionality</param>
    /// <param name="defaultRequestLimitMs">Default time frame on the number of requests for all endpoints</param>
    /// <param name="defaultRequestLimitCount">Limit on the consecutive number of requests for all endpoints</param>
    /// <param name="endpointLimits">Limit list for specific endpoint, null value is allowed</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public RateLimiterConfig(bool requestLimiterEnabled,
                             int defaultRequestLimitMs,
                             int defaultRequestLimitCount,
                             List<EndpointLimiterConfig>? endpointLimits)
    {
        if (defaultRequestLimitCount < 0) throw new ArgumentOutOfRangeException(nameof(defaultRequestLimitCount));
        if (defaultRequestLimitMs < 0) throw new ArgumentOutOfRangeException(nameof(defaultRequestLimitMs));

        DefaultRequestLimitCount = defaultRequestLimitCount;
        DefaultRequestLimitMs = defaultRequestLimitMs;
        EndpointLimits = endpointLimits;
        RequestLimiterEnabled = requestLimiterEnabled;
    }
}