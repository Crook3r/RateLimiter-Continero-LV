namespace RateLimiter.Tests.ConfigurationTests.RateLimiterConfigTests;

using Configuration;

[TestClass]
public class ConstructorTest
{
    [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void CountIsOutOfRange()
    {
        var rateLimiterConfig = new RateLimiterConfig(true, 1000, -1, null);
    }

    [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void MsIsOutOfRange()
    {
        var rateLimiterConfig = new RateLimiterConfig(true, -1, 5, null);
    }

    [TestMethod]
    public void CorrectCall()
    {
        var rateLimiterConfig = new RateLimiterConfig(true, 1000, 5, null);
    }
}