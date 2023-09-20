using System.Net;
using DruidsCornerApiClient.Models;
using DruidsCornerApiClient.Models.Exceptions;
using DruidsCornerApiClient.Services;
using DruidsCornerApp.Services.Authentication;
using DruidsCornerApp.Services.Config;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;

namespace DruidsCornerAppUnitTests.DruidsCornerApiClientTests;

public class ConfigProviderTests
{
    #region Configuration tests

    [Test]
    public async Task TestCanReadConfig()
    {
        var provider = new LocalAuthConfigProvider();
        var authConfig = await provider.GetAuthConfigAsync();

        Assert.That(authConfig.ApiKey, Is.Not.EqualTo(""));
    }
    #endregion

}