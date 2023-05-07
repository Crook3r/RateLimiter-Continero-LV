namespace RateLimiter.Tests.EndpointLimiterTests;

[TestClass]
public class ConstructorTest
{
    [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void InvalidMs()
    {
        var endpointLimiter = new EndpointLimiter(0, 5);
    }

    [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void InvalidCount()
    {
        var endpointLimiter = new EndpointLimiter(1000, 0);
    }
}