namespace DruidsCornerAppUnitTests;
using DruidsCornerApp.Services;

public class AuthConfigProviderTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task TestCanReadConfig()
    {
        var provider = new LocalAuthConfigProvider();
        var authConfig = await provider.GetAuthConfigAsync();
        
        Assert.That(authConfig.ApiKey, Is.Not.EqualTo(""));
    }
}