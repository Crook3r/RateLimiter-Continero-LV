namespace RateLimiter.Tests.ConfigurationTests.EndpointLimitConfigTests;

using Configuration;

[TestClass]
public class ConstructorTest
{
    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void EndpointIsNull()
    {
        var endpointLimitConfig = new EndpointLimiterConfig(null, 1000, 5);
    }

    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void EndpointIsEmpty()
    {
        var endpointLimitConfig = new EndpointLimiterConfig(string.Empty, 1000, 5);
    }

    [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void CountIsOutOfRange()
    {
        var endpointLimitConfig = new EndpointLimiterConfig("Endpoint1", 1000, -1);
    }

    [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void MsIsOutOfRange()
    {
        var endpointLimitConfig = new EndpointLimiterConfig("Endpoint1", -1, 5);
    }

    [TestMethod]
    public void CorrectCall()
    {
        var endpointLimitConfig = new EndpointLimiterConfig("Endpoint1", 1000, 5);
    }
}