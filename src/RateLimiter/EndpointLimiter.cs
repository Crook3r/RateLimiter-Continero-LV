using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RateLimiter.Tests")]
namespace RateLimiter;

/// <summary>
/// Limiter that is filtering access to single endpoint based on its configuration
/// </summary>
internal class EndpointLimiter
{
    private readonly Dictionary<string, List<DateTime>> _ipAddressVisits;
    private readonly int _requestLimitMs;
    private readonly int _requestLimitCount;

    /// <summary>
    /// Initialize instance of the <see cref="T:RateLimiter.EndpointLimiter" />
    /// </summary>
    /// <param name="requestLimitMs">Default timeframe to the number of endpoint requests</param>
    /// <param name="requestLimitCount">Limit on the number of consecutive requests in a time frame for an endpoint</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal EndpointLimiter(int requestLimitMs, int requestLimitCount)
    {
        if (requestLimitMs <= 0) throw new ArgumentOutOfRangeException(nameof(requestLimitMs));
        if (requestLimitCount <= 0) throw new ArgumentOutOfRangeException(nameof(requestLimitCount));

        _requestLimitMs = requestLimitMs;
        _requestLimitCount = requestLimitCount;
        _ipAddressVisits = new Dictionary<string, List<DateTime>>();
    }

    /// <summary>
    /// Check, if request from IpAddress is allowed
    /// </summary>
    /// <param name="ipAddress">Requests IP address</param>
    /// <param name="timestamp">Requests timestamp</param>
    /// <returns>If the request is allowed</returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal bool CheckIsAllowed(string ipAddress, DateTime timestamp)
    {
        if (string.IsNullOrWhiteSpace(ipAddress)) throw new ArgumentNullException(nameof(ipAddress));

        if (_ipAddressVisits.ContainsKey(ipAddress))
        {
            var requestedEndpoint = _ipAddressVisits[ipAddress];

            var millisecondsFromLatestRequest = (timestamp - requestedEndpoint.First()).TotalMilliseconds;

            if (millisecondsFromLatestRequest <= _requestLimitMs && requestedEndpoint.Count == _requestLimitCount) return false;

            requestedEndpoint.Add(timestamp);
            if (requestedEndpoint.Count > _requestLimitCount) requestedEndpoint.RemoveAt(0);

            return true;
        }

        var value = new List<DateTime> { timestamp };
        _ipAddressVisits.Add(ipAddress, value);
        return true;
    }
}