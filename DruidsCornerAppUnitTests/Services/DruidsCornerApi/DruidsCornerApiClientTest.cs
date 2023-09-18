using DruidsCornerApp.Models.Login;
using DruidsCornerApp.Services;
using DruidsCornerApp.Services.Authentication;
using DruidsCornerApp.Services.DruidsCornerApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace DruidsCornerAppUnitTests.Services;

public class DruidsCornerApiClientTest
{
    [SetUp]
    public void Setup()
    {
    }

    private IConfiguration GetFakeConfiguration()
    {
        // Inspired from https://stackoverflow.com/a/55497919
        var myConfiguration = new Dictionary<string, string>
        {
            {"Key1", "Value1"},
            {"Nested:Key1", "NestedValue1"},
            {"Nested:Key2", "NestedValue2"}
        };

        // Disabling because this just some test code, won't pose any real issue.
#pragma warning disable CS8620
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(myConfiguration)
                            .Build();
        return configuration;
#pragma warning restore CS8620
    }
    
    
    [Test]
    public async Task TestCanReadConfig()
    {
        var provider = new LocalAuthConfigProvider();
        var authConfig = await provider.GetAuthConfigAsync();
        
        Assert.That(authConfig.ApiKey, Is.Not.EqualTo(""));
    }

    /// <summary>
    /// Will ping the remote web service for real, this test requires the Env Var "ACCESS_TOKEN" to be
    /// a valid Token used for this application stack
    /// </summary>
    public async Task TestGetAllRecipes_RealCalls()
    {
        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<DruidsCornerApiClient>>();
        var config = GetFakeConfiguration();
        var mockSecureStorageService = new Mock<ISecureStorageService>();
        
        // Retrieve a real token from execution environment ?
        var token = System.Environment.GetEnvironmentVariable("ACCESS_TOKEN");
        Assert.That(token, Is.Not.Null);

        // Mock the secure storage service and make it return the token that we have in env var instead
        mockSecureStorageService.Setup(storage => storage.GetAsync(AccountKeys.TokenKey)).ReturnsAsync(token);

        var httpClient = new PlatformHttpClient("", "");
        var client = new DruidsCornerApiClient(mockedLogger.Object, config, mockSecureStorageService.Object, httpClient);
    }
}