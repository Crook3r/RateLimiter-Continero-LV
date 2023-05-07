namespace RateLimiter.Tests.RateLimiterTests;

using Configuration;

[TestClass]
public class InitializeTest
{
    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void ConfigIsEmpty()
    {
        RateLimiterConfig config = null!;
        var limiter = new RateLimiter();

        limiter.Initialize(config);
    }

    [TestMethod]
    public void CheckIsInitializedWithoutExceptionWithNullEndpointLimits()
    {

        var config = new RateLimiterConfig(true, 4, 1000, null);

        var limiter = new RateLimiter();
        limiter.Initialize(config);
    }

    [TestMethod]
    public void CheckIsInitializedWithoutExceptionWithEmptyEndpointLimits()
    {
        var endpointLimitConfigs = new List<EndpointLimiterConfig>();
        var config = new RateLimiterConfig(true, 4, 1000, endpointLimitConfigs);

        var limiter = new RateLimiter();
        limiter.Initialize(config);
    }

    [TestMethod]
    public void CheckIsInitializedWithoutExceptionWithOnEndpointLimits()
    {
        var endpointLimitConfigs = new List<EndpointLimiterConfig>
        {
            new("Endpoint1", 4, 500)
        };
        var config = new RateLimiterConfig(true, 4, 1000, endpointLimitConfigs);

        var limiter = new RateLimiter();
        limiter.Initialize(config);
    }
}