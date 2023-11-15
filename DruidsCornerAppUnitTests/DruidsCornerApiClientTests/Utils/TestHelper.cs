using DruidsCornerApiClient.Models;
using DruidsCornerApiClient.Services.Interfaces;
using DruidsCornerApp.Services.Config;
using Microsoft.Extensions.Logging;
using Moq;

namespace DruidsCornerAppUnitTests.DruidsCornerApiClientTests;

public class TestHelper
{
    public static async Task<ClientConfiguration> GetConfigAsync()
    {
        var mockedConfigLogger = new Mock<ILogger<ConfigProvider>>();
        var configProvider = new ConfigProvider(mockedConfigLogger.Object);
        var config = await configProvider.GetConfigAsync();
        Assert.That(config, Is.Not.Null);
        Assert.That(string.IsNullOrEmpty(config!.Domain), Is.False);
        return config;
    }

    public static string GetEnv(string envVarName)
    {
        var value = System.Environment.GetEnvironmentVariable(envVarName);
        Assert.That(value, Is.Not.Null);
        return value!;
    }
    
    /// <summary>
    /// Detects whether current environment is hosted within a build pipeline or not
    /// This is used as a trigger to skip tests, as some tests can generate some unwanted load and are noticeably longer to run
    /// than regular stubbed unit tests.
    /// </summary>
    /// <returns></returns>
    public static bool IsTriggeredFromPipeline()
    {
        return !string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("CI"));
    }
    
}