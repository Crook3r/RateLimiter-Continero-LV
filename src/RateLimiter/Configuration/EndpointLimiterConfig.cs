namespace RateLimiter.Configuration;

/// <summary>
/// Configuration for <see cref="T:RateLimiter.EndpointLimiter" />
/// </summary>
public class EndpointLimiterConfig
{
    /// <summary>
    /// Specific endpoint line trace
    /// </summary>
    internal string Endpoint { get; }
    /// <summary>
    /// Default timeframe to the number of endpoint requests
    /// </summary>
    internal int RequestLimitMs { get; }
    /// <summary>
    /// Limit on the number of consecutive requests in a time frame for an endpoint
    /// </summary>
    internal int RequestLimitCount { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="T:RateLimiter.Configuration.EndpointLimiterConfig" />.
    /// </summary>
    /// <param name="endpoint">Specific endpoint line trace</param>
    /// <param name="requestLimitMs">Default timeframe to the number of endpoint requests</param>
    /// <param name="requestLimitCount">Limit on the number of consecutive requests in a time frame for an endpoint</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public EndpointLimiterConfig(string endpoint,
                                 int requestLimitMs,
                                 int requestLimitCount)
    {
        if (string.IsNullOrEmpty(endpoint)) throw new ArgumentNullException(nameof(endpoint));
        if (requestLimitCount < 0) throw new ArgumentOutOfRangeException(nameof(requestLimitCount));
        if (requestLimitMs < 0) throw new ArgumentOutOfRangeException(nameof(requestLimitMs));

        Endpoint = endpoint;
        RequestLimitCount = requestLimitCount;
        RequestLimitMs = requestLimitMs;
    }
}