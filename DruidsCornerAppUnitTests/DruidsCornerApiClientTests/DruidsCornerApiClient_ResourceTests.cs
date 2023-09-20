using System.Net;
using System.Net.Mime;
using DruidsCornerApiClient.Models;
using DruidsCornerApiClient.Models.Exceptions;
using DruidsCornerApiClient.Services;
using DruidsCornerApiClient.Services.Interfaces;
using DruidsCornerApp.Services.Authentication;
using DruidsCornerApp.Services.Config;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;

namespace DruidsCornerAppUnitTests.DruidsCornerApiClientTests;

public class ClientResourceTest
{
    #region GetImage tests

    [Test]
    public async Task TestGetImage_RealCalls()
    {
        // Skip if triggered from a build pipeline (RealCalls generate unwanted load to WebApis)
        // Plus they are significantly slower due to their nature, so only keep them for local development
        if (TestHelper.IsTriggeredFromPipeline())
        {
            Assert.Ignore();
        }
        
        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<IBaseClient>>();
        var config = await TestHelper.GetConfigAsync();
        
        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);

        // Don't need a Platform client for that as DruidsCornerApi doesn't care about terminals (but it needs an appropriate JWT token however)
        var httpClient = new HttpClient();
        var client = new ResourceClient(mockedLogger.Object, httpClient, config!);

        var imageFileStream = await client.GetImageAsync(3, token);
        Assert.That(imageFileStream, Is.Not.Null);
        Assert.That(imageFileStream!.Stream, Is.Not.Null);
        Assert.That(imageFileStream!.Format, Is.EqualTo("png"));
    }

    [Test]
    public async Task TestGetImage_HttpErrors()
    {
        var mockedHttpMessageHandler = new MockHttpMessageHandler();

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<IBaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);
        var domain = TestHelper.GetEnv(TestContstants.WebApiDomainEnvVarName);
        var endpoint = $"https://{domain}/resources/image*";

        var mockedHttpClient = mockedHttpMessageHandler.ToHttpClient();
        mockedHttpMessageHandler.When(endpoint)
                                .Respond(req => new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.NotFound,
                                    Content = new StringContent("Whoops !")
                                });

        var client = new ResourceClient(mockedLogger.Object, mockedHttpClient, config);

        // Should return null reference when Error 500 Internal Server Error is caught
        var imageStream = await client.GetImageAsync(3, token);
        Assert.That(imageStream, Is.Null);

        mockedHttpMessageHandler.When(endpoint)
                                .Respond(req => new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.InternalServerError,
                                    Content = new StringContent("Whoops !")
                                });

        // Now, it should break because the token is missing from current context
        imageStream = await client.GetImageAsync(3, token);
        Assert.That(imageStream, Is.Null);
        
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
                _ = await client.GetImageAsync(3, token);
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
    
     #region GetPdfPage tests

    [Test]
    public async Task TestGetPdfPage_RealCalls()
    {
        // Skip if triggered from a build pipeline (RealCalls generate unwanted load to WebApis)
        // Plus they are significantly slower due to their nature, so only keep them for local development
        if (TestHelper.IsTriggeredFromPipeline())
        {
            Assert.Ignore();
        }
        
        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<IBaseClient>>();
        var config = await TestHelper.GetConfigAsync();
        
        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);

        // Don't need a Platform client for that as DruidsCornerApi doesn't care about terminals (but it needs an appropriate JWT token however)
        var httpClient = new HttpClient();
        var client = new ResourceClient(mockedLogger.Object, httpClient, config!);

        var imageFileStream = await client.GetPdfPageAsync(3, token);
        Assert.That(imageFileStream, Is.Not.Null);
        Assert.That(imageFileStream!.Stream, Is.Not.Null);
        Assert.That(imageFileStream!.Name, Is.EqualTo("beer.pdf"));
    }

    [Test]
    public async Task TestGetPdfPage_HttpErrors()
    {
        var mockedHttpMessageHandler = new MockHttpMessageHandler();

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<IBaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);
        var domain = TestHelper.GetEnv(TestContstants.WebApiDomainEnvVarName);
        var endpoint = $"https://{domain}/resources/image*";

        var mockedHttpClient = mockedHttpMessageHandler.ToHttpClient();
        mockedHttpMessageHandler.When(endpoint)
                                .Respond(req => new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.NotFound,
                                    Content = new StringContent("Whoops !")
                                });

        var client = new ResourceClient(mockedLogger.Object, mockedHttpClient, config);

        // Should return null reference when Error 500 Internal Server Error is caught
        var pdfStream = await client.GetPdfPageAsync(3, token);
        Assert.That(pdfStream, Is.Null);

        mockedHttpMessageHandler.When(endpoint)
                                .Respond(req => new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.InternalServerError,
                                    Content = new StringContent("Whoops !")
                                });

        // Now, it should break because the token is missing from current context
        pdfStream = await client.GetPdfPageAsync(3, token);
        Assert.That(pdfStream, Is.Null);
        
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
                _ = await client.GetPdfPageAsync(3, token);
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
}