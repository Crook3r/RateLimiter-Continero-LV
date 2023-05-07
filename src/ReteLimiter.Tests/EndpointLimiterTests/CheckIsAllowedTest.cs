namespace RateLimiter.Tests.EndpointLimiterTests;

[TestClass]
public class CheckIsAllowedTest
{
    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void InvalidIpAddress()
    {
        var limiter = new EndpointLimiter(1000, 4);
        
        Assert.IsTrue(limiter.CheckIsAllowed(string.Empty, new DateTime(2023, 1, 1, 0, 0, 0, 0)));
    }

    [TestMethod]
    public void SingleIp_LastCheckIs1MsBelowTimeLimit()
    {
        var limiter = new EndpointLimiter(1000, 4);
        const string ipAddress1 = "127.0.0.1";

        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 1)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 2)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 3)));

        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 999)));
    }

    [TestMethod]
    public void SingleIp_LastCheckIs1MsOnTimeLimit()
    {
        var limiter = new EndpointLimiter(1000, 4);
        const string ipAddress1 = "127.0.0.1";

        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 1)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 2)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 3)));

        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 1, 0)));
    }

    [TestMethod]
    public void SingleIp_LastCheckIs1MsOverTimeLimit()
    {
        var limiter = new EndpointLimiter(1000, 4);
        const string ipAddress1 = "127.0.0.1";

        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 1)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 2)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 3)));

        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 1, 1)));
    }

    [TestMethod]
    public void MultipleIps_CheckIsCorrectlyApplied()
    {
        var limiter = new EndpointLimiter(1000, 4);
        const string ipAddress1 = "127.0.0.1";
        const string ipAddress2 = "127.0.0.2";

        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress2, new DateTime(2023, 1, 1, 0, 0, 0, 0)));

        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 1)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress2, new DateTime(2023, 1, 1, 0, 0, 0, 1)));

        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 2)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress2, new DateTime(2023, 1, 1, 0, 0, 0, 2)));

        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 3)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress2, new DateTime(2023, 1, 1, 0, 0, 0, 3)));

        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 1, 0)));
        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress2, new DateTime(2023, 1, 1, 0, 0, 1, 0)));
    }

    [TestMethod]
    public void SingleIp_MultipleRequestsAfter100Ms()
    {
        var limiter = new EndpointLimiter(1000, 4);
        const string ipAddress1 = "127.0.0.1";

        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 0)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 100)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 200)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 300)));

        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 400)));
        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 500)));
        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 600)));
        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 700)));
        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 800)));
        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 0, 900)));

        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 1, 0)));

        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 1, 100)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 1, 200)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 1, 300)));
        Assert.IsTrue(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 1, 400)));

        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 1, 500)));
        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 1, 600)));
        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 1, 700)));
        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 1, 800)));
        Assert.IsFalse(limiter.CheckIsAllowed(ipAddress1, new DateTime(2023, 1, 1, 0, 0, 1, 900)));
    }


}