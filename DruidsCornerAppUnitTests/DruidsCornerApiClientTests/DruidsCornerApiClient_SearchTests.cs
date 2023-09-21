using System.Net;
using Moq;
using RichardSzalay.MockHttp;
using DruidsCornerApiClient.Models.Exceptions;
using DruidsCornerApiClient.Models.Search;
using DruidsCornerApiClient.Services;
using DruidsCornerApiClient.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace DruidsCornerAppUnitTests.DruidsCornerApiClientTests;

public class ClientSearchTest
{
    #region Test helpers

    public async Task TestEndpointAgainstCommonHttpErrors<T>(SearchClient client,
                                                             MockHttpMessageHandler mockedHttpMessageHandler,
                                                             string endpoint,
                                                             Func<SearchClient, Task<List<T>?>> delegated
    )
    {
        mockedHttpMessageHandler.When(endpoint)
                                .Respond(req => new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.NotFound,
                                    Content = new StringContent("Whoops !")
                                });

        // Should return null reference when Error 500 Internal Server Error is caught
        var props = await delegated.Invoke(client);
        Assert.That(props, Is.Null);

        mockedHttpMessageHandler.When(endpoint)
                                .Respond(req => new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.InternalServerError,
                                    Content = new StringContent("Whoops !")
                                });
        props = await delegated.Invoke(client);
        Assert.That(props, Is.Null);

        // Check Unauthorized and Forbidden are supported as expected
        var statusCodes = new List<HttpStatusCode> { HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized };
        foreach (var statusCode in statusCodes)
        {
            mockedHttpMessageHandler.When(endpoint)
                                    .Respond(req => new HttpResponseMessage()
                                    {
                                        StatusCode = statusCode,
                                        Content = new StringContent("Whoops !")
                                    });
            // Should return null reference when Error 401/403 Unauthorized / Authentication failure errors
            try
            {
                _ = await delegated.Invoke(client);
            }
            catch (ClientException ex)
            {
                Assert.That(ex.FailureMode, Is.EqualTo(FailureModes.AuthenticationFailure));
            }
            catch (Exception)
            {
                Assert.Fail("Should not get there!");
            }
        }
    }

    #endregion

    #region Search Hops tests

    [Test]
    public async Task TestSearchHops_RealCalls()
    {
        // Skip if triggered from a build pipeline (RealCalls generate unwanted load to WebApis)
        // Plus they are significantly slower due to their nature, so only keep them for local development
        if (TestHelper.IsTriggeredFromPipeline())
        {
            Assert.Ignore();
        }

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<BaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);

        // Don't need a Platform client for that as DruidsCornerApi doesn't care about terminals (but it needs an appropriate JWT token however)
        var httpClient = new HttpClient();
        var client = new SearchClient(mockedLogger.Object, httpClient, config!);

        var hopsNames = new List<string>()
        {
            "Magnum",
            "Centennial",
            "Chinook"
        };
        var hopsPropList = await client.SearchHopsByNameAsync(hopsNames, token);
        Assert.That(hopsPropList, Is.Not.Null);
    }

    [Test]
    public async Task TestSearchHops_HttpErrors()
    {
        var mockedHttpMessageHandler = new MockHttpMessageHandler();
        var mockedHttpClient = mockedHttpMessageHandler.ToHttpClient();

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<BaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);
        var endpoint = $"https://{config.Domain}/search/hops*";

        var client = new SearchClient(mockedLogger.Object, mockedHttpClient, config);
        await TestEndpointAgainstCommonHttpErrors(client,
                                                  mockedHttpMessageHandler,
                                                  endpoint,
                                                  searchClient => searchClient.SearchHopsByNameAsync(new List<string>() { "plop" }, token));
    }

    #endregion

    #region Search Malts tests

    [Test]
    public async Task TestSearchMalts_RealCalls()
    {
        // Skip if triggered from a build pipeline (RealCalls generate unwanted load to WebApis)
        // Plus they are significantly slower due to their nature, so only keep them for local development
        if (TestHelper.IsTriggeredFromPipeline())
        {
            Assert.Ignore();
        }

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<BaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);

        // Don't need a Platform client for that as DruidsCornerApi doesn't care about terminals (but it needs an appropriate JWT token however)
        var httpClient = new HttpClient();
        var client = new SearchClient(mockedLogger.Object, httpClient, config!);

        var maltsNames = new List<string>()
        {
            "Chocolate",
            "Extra pale"
        };
        var maltsPropList = await client.SearchMaltsByNameAsync(maltsNames, token);
        Assert.That(maltsPropList, Is.Not.Null);
    }

    [Test]
    public async Task TestSearchMalts_HttpErrors()
    {
        var mockedHttpMessageHandler = new MockHttpMessageHandler();
        var mockedHttpClient = mockedHttpMessageHandler.ToHttpClient();

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<BaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);
        var endpoint = $"https://{config.Domain}/search/malts*";

        var client = new SearchClient(mockedLogger.Object, mockedHttpClient, config);
        await TestEndpointAgainstCommonHttpErrors(client,
                                                  mockedHttpMessageHandler,
                                                  endpoint,
                                                  searchClient => searchClient.SearchMaltsByNameAsync(new List<string>() { "plop" }, token));
    }

    #endregion

    #region Search Yeasts tests

    [Test]
    public async Task TestSearchYeasts_RealCalls()
    {
        // Skip if triggered from a build pipeline (RealCalls generate unwanted load to WebApis)
        // Plus they are significantly slower due to their nature, so only keep them for local development
        if (TestHelper.IsTriggeredFromPipeline())
        {
            Assert.Ignore();
        }

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<BaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);

        // Don't need a Platform client for that as DruidsCornerApi doesn't care about terminals (but it needs an appropriate JWT token however)
        var httpClient = new HttpClient();
        var client = new SearchClient(mockedLogger.Object, httpClient, config!);

        var queryNames = new List<string>()
        {
            "WYeast 1056",
            "WLP 500"
        };
        var returnedPropList = await client.SearchYeastsByNameAsync(queryNames, token);
        Assert.That(returnedPropList, Is.Not.Null);
    }

    [Test]
    public async Task TestSearchYeasts_HttpErrors()
    {
        var mockedHttpMessageHandler = new MockHttpMessageHandler();
        var mockedHttpClient = mockedHttpMessageHandler.ToHttpClient();

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<BaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);
        var endpoint = $"https://{config.Domain}/search/yeasts*";

        var client = new SearchClient(mockedLogger.Object, mockedHttpClient, config);
        await TestEndpointAgainstCommonHttpErrors(client,
                                                  mockedHttpMessageHandler,
                                                  endpoint,
                                                  searchClient => searchClient.SearchYeastsByNameAsync(new List<string>() { "plop" }, token));
    }

    #endregion

    #region Search Styles tests

    [Test]
    public async Task TestSearchStyles_RealCalls()
    {
        // Skip if triggered from a build pipeline (RealCalls generate unwanted load to WebApis)
        // Plus they are significantly slower due to their nature, so only keep them for local development
        if (TestHelper.IsTriggeredFromPipeline())
        {
            Assert.Ignore();
        }

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<BaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);

        // Don't need a Platform client for that as DruidsCornerApi doesn't care about terminals (but it needs an appropriate JWT token however)
        var httpClient = new HttpClient();
        var client = new SearchClient(mockedLogger.Object, httpClient, config!);

        var queryNames = new List<string>()
        {
            "India Pale Ale",
            "Imperial Stout"
        };
        var returnedPropList = await client.SearchStylesByNameAsync(queryNames, token);
        Assert.That(returnedPropList, Is.Not.Null);
    }

    [Test]
    public async Task TestSearchStyles_HttpErrors()
    {
        var mockedHttpMessageHandler = new MockHttpMessageHandler();
        var mockedHttpClient = mockedHttpMessageHandler.ToHttpClient();

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<BaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);
        var endpoint = $"https://{config.Domain}/search/styles*";

        var client = new SearchClient(mockedLogger.Object, mockedHttpClient, config);
        await TestEndpointAgainstCommonHttpErrors(client,
                                                  mockedHttpMessageHandler,
                                                  endpoint,
                                                  searchClient => searchClient.SearchStylesByNameAsync(new List<string>() { "plop" }, token));
    }

    #endregion

    #region Search Multiple Queries tests

    [Test]
    public async Task TestSearchMultipleQueries_RealCalls()
    {
        // Skip if triggered from a build pipeline (RealCalls generate unwanted load to WebApis)
        // Plus they are significantly slower due to their nature, so only keep them for local development
        if (TestHelper.IsTriggeredFromPipeline())
        {
            Assert.Ignore();
        }

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<BaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);

        // Don't need a Platform client for that as DruidsCornerApi doesn't care about terminals (but it needs an appropriate JWT token however)
        var httpClient = new HttpClient();
        var client = new SearchClient(mockedLogger.Object, httpClient, config!);

        var queries = new Queries()
        {
            Abv = new Range<float>(3, 10),
            HopList = new List<string>() { "Centennial", "Citra", "Chinook", "Mosaic" },
            MaltList = new List<string>() { "Extra Pale", "Caramalt", "Carared" },
        };
        var candidates = await client.SearchAllCandidatesAsync(queries, token);
        Assert.That(candidates, Is.Not.Null);
    }

    #endregion
}