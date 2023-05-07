namespace RateLimiter.Tests.RateLimiterTests;

using Configuration;

[TestClass]
public class CheckRequestIsAllowedTest
{
    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void InvalidEndpoint()
    {
        const string ipAddress1 = "127.0.0.1";
        
        var limiter = new RateLimiter();

        limiter.CheckRequestIsAllowed(string.Empty, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0));
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void InvalidIpAddress()
    {
        const string endpoint1 = "endpoint1";

        var limiter = new RateLimiter();

        limiter.CheckRequestIsAllowed(endpoint1, string.Empty, new DateTime(2023, 1, 1, 0, 0, 0, 0));
    }

    [TestMethod, ExpectedException(typeof(InvalidOperationException))]
    public void LimiterIsNotInitialized()
    {
        const string ipAddress1 = "127.0.0.1";
        const string endpoint1 = "endpoint1";

        var limiter = new RateLimiter();
        limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0));
    }

    [TestMethod]
    public void RequestLimiterIsDisabled()
    {
        const string ipAddress1 = "127.0.0.1";
        const string endpoint1 = "endpoint1";

        var config = new RateLimiterConfig(false, 1000, 4, null);

        var limiter = new RateLimiter();
        limiter.Initialize(config);

        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 100)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 200)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 300)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 400)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 500)));
    }

    [TestMethod]
    public void NoSpecificEndpoint_RequestsToSingleEndpointFromSingleIp()
    {
        const string ipAddress1 = "127.0.0.1";
        const string endpoint1 = "endpoint1";

        var config = new RateLimiterConfig(true, 1000, 4, null);

        var limiter = new RateLimiter();
        limiter.Initialize(config);

        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 100)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 200)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 300)));
        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 400)));
        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 500)));
    }

    [TestMethod]
    public void NoSpecificEndpoint_RequestsToTwoEndpointsFromSingleIp()
    {
        const string ipAddress1 = "127.0.0.1";
        const string endpoint1 = "endpoint1";
        const string endpoint2 = "endpoint2";

        var config = new RateLimiterConfig(true, 1000, 4, null);

        var limiter = new RateLimiter();
        limiter.Initialize(config);

        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint2, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 100)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 200)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint2, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 300)));
        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 400)));
        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint2, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 500)));
    }

    [TestMethod]
    public void NoSpecificEndpoint_RequestsToSingleEndpointFromMultipleIps()
    {
        const string ipAddress1 = "127.0.0.1";
        const string ipAddress2 = "127.0.0.2";
        const string endpoint1 = "endpoint1";

        var config = new RateLimiterConfig(true, 1000, 4, null);

        var limiter = new RateLimiter();
        limiter.Initialize(config);

        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress2, new DateTime(2023, 1, 1, 0, 0, 0, 0)));

        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 100)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress2, new DateTime(2023, 1, 1, 0, 0, 0, 100)));

        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 200)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress2, new DateTime(2023, 1, 1, 0, 0, 0, 200)));

        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 300)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress2, new DateTime(2023, 1, 1, 0, 0, 0, 300)));

        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 400)));
        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint1, ipAddress2, new DateTime(2023, 1, 1, 0, 0, 0, 400)));
    }

    [TestMethod]
    public void SpecificEndpoint_RequestsToSingleEndpointFromSingleIp()
    {
        const string ipAddress1 = "127.0.0.1";
        const string endpoint1 = "endpoint1";

        var endpointLimitConfigs = new List<EndpointLimiterConfig>
        {
            new(endpoint1, 500, 2)
        };
        var config = new RateLimiterConfig(true, 1000, 4, endpointLimitConfigs);

        var limiter = new RateLimiter();
        limiter.Initialize(config);

        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 100)));
        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 200)));
        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 300)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 600)));
    }

    [TestMethod]
    public void SpecificEndpoint_RequestsToTwoEndpointsFromSingleIp()
    {
        const string ipAddress1 = "127.0.0.1";
        const string endpoint1 = "endpoint1";
        const string endpoint2 = "endpoint2";

        var endpointLimitConfigs = new List<EndpointLimiterConfig>
        {
            new(endpoint1, 500, 2)
        };
        var config = new RateLimiterConfig(true, 1000, 4, endpointLimitConfigs);

        var limiter = new RateLimiter();
        limiter.Initialize(config);

        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint2, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0)));

        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 100)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint2, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 100)));

        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 200)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint2, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 200)));

        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 300)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint2, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 300)));

        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 400)));
        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint2, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 400)));
    }

    [TestMethod]
    public void SpecificEndpoint_RequestsToSingleEndpointFromMultipleIps()
    {
        const string ipAddress1 = "127.0.0.1";
        const string ipAddress2 = "127.0.0.2";
        const string endpoint1 = "endpoint1";

        var endpointLimitConfigs = new List<EndpointLimiterConfig>
        {
            new(endpoint1, 500, 2)
        };
        var config = new RateLimiterConfig(true, 1000, 4, endpointLimitConfigs);


        var limiter = new RateLimiter();
        limiter.Initialize(config);

        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress2, new DateTime(2023, 1, 1, 0, 0, 0, 0)));

        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 100)));
        Assert.IsTrue(limiter.CheckRequestIsAllowed(endpoint1, ipAddress2, new DateTime(2023, 1, 1, 0, 0, 0, 100)));

        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint1, ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 200)));
        Assert.IsFalse(limiter.CheckRequestIsAllowed(endpoint1, ipAddress2, new DateTime(2023, 1, 1, 0, 0, 0, 200)));
    }
}