using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RateLimiter.Tests")]
namespace RateLimiter;

using Configuration;

/// <summary>
/// Limiter that is filtering access to endpoints based on its configuration
/// </summary>
public class RateLimiter
{
    private const string AllEndpointsKey = "/";

    private Dictionary<string, EndpointLimiter>? _endpoints;
    private RateLimiterConfig? _config;

    /// <summary>
    /// Initializes limiter with <see cref="T:RateLimiter.Configuration.RateLimiterConfig" />
    /// </summary>
    /// <param name="config">Configuration of limiter</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Initialize(RateLimiterConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));

        _endpoints = new Dictionary<string, EndpointLimiter>
        {
            { AllEndpointsKey, new EndpointLimiter(config.DefaultRequestLimitMs, config.DefaultRequestLimitCount) }
        };

        if (config.EndpointLimits == null) return;

        foreach (var endpointLimit in config.EndpointLimits)
        {
            _endpoints.Add(endpointLimit.Endpoint,
                           new EndpointLimiter(endpointLimit.RequestLimitMs, endpointLimit.RequestLimitCount));
        }
    }

    /// <summary>
    /// Check, if request from IpAddress to specific Endpoint is allowed
    /// </summary>
    /// <param name="endpoint">Specific endpoint</param>
    /// <param name="ipAddress">Requests IP address</param>
    /// <param name="timestamp">Requests timestamp</param>
    /// <returns>If the request is allowed</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public bool CheckRequestIsAllowed(string endpoint, string ipAddress, DateTime timestamp)
    {
        if (string.IsNullOrWhiteSpace(endpoint)) throw new ArgumentNullException(nameof(endpoint));
        if (string.IsNullOrWhiteSpace(ipAddress)) throw new ArgumentNullException(nameof(ipAddress));

        if (_config == null || _endpoints == null)
            throw new
                InvalidOperationException($"{nameof(RateLimiter)} must be initialized first with {nameof(Initialize)} method.");

        if (!_config.RequestLimiterEnabled) return true;

        var requestedEndpoint = _endpoints.ContainsKey(endpoint) ? _endpoints[endpoint] : _endpoints[AllEndpointsKey];
        return requestedEndpoint.CheckIsAllowed(ipAddress, timestamp);
    }
}