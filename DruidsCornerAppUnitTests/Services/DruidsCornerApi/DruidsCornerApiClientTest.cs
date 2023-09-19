using System.Runtime.InteropServices.JavaScript;
using DruidsCornerApp.Models.Config;
using DruidsCornerApp.Models.Login;
using DruidsCornerApp.Services;
using DruidsCornerApp.Services.Authentication;
using DruidsCornerApp.Services.Config;
using DruidsCornerApp.Services.DruidsCornerApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace DruidsCornerAppUnitTests.Services;

public class DruidsCornerApiClientTest
{
    private const string AccessTokenEnvVarName = "ACCESS_TOKEN";
    
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

    /// <summary>
    /// Will ping the remote web service for real, this test requires the Env Var "ACCESS_TOKEN" to be
    /// a valid Token used for this application stack
    /// </summary>
    [Test]
    [Ignore("Not ready yet")]
    public async Task TestGetAllRecipes_StubbedCallsErrorResponse()
    {
        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<DruidsCornerApiClient>>();
        var mockConfig = new Mock<IConfigProvider>();
        var mockSecureStorageService = new Mock<ISecureStorageService>();

        var config = new DruidsCornerApiConfig()
        {
            Domain = "fakedomain.com"
        };

        mockConfig.Setup(provider => provider.GetConfigAsync(It.IsAny<bool>())).ReturnsAsync(config);
        
        // Retrieve a real token from execution environment ?
        var token = System.Environment.GetEnvironmentVariable(AccessTokenEnvVarName);
        Assert.That(token, Is.Not.Null);

        // Mock the secure storage service and make it return the token that we have in env var instead
        mockSecureStorageService.Setup(storage => storage.GetAsync(AccountKeys.TokenKey)).ReturnsAsync(token);

        var httpClient = new PlatformHttpClient("", "");
        var client = new DruidsCornerApiClient(mockedLogger.Object, mockSecureStorageService.Object, httpClient, mockConfig.Object);
    }
    
    [Test]
    public async Task TestGetSingleRecipe_RealCalls()
    {
        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<DruidsCornerApiClient>>();
        var mockedConfigLogger = new Mock<ILogger<ConfigProvider>>();
        
        var configProvider = new ConfigProvider(mockedConfigLogger.Object);
        var mockSecureStorageService = new Mock<ISecureStorageService>();

        var config = await configProvider.GetConfigAsync();
        Assert.That(config, Is.Not.Null);
        Assert.That(string.IsNullOrEmpty(config!.Domain), Is.False );
        
        // Retrieve a real token from execution environment ?
        var token = System.Environment.GetEnvironmentVariable(AccessTokenEnvVarName);
        Assert.That(token, Is.Not.Null);

        // Mock the secure storage service and make it return the token that we have in env var instead
        mockSecureStorageService.Setup(storage => storage.GetAsync(AccountKeys.TokenKey)).ReturnsAsync(token);

        // Don't need a Platform client for that as DruidsCornerApi doesn't care about terminals (but it needs an appropriate JWT token however)
        var httpClient = new HttpClient();
        var client = new DruidsCornerApiClient(mockedLogger.Object, mockSecureStorageService.Object, httpClient, configProvider);

        var recipe = await client.GetRecipeByNumberAsync(1);
        Assert.That(recipe, Is.Not.Null);
    }
}