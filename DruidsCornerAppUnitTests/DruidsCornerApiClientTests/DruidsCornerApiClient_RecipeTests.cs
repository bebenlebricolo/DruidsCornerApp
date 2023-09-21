using System.Net;
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

public class ClientRecipeTest
{
    #region GetAllRecipes Tests

    [Test]
    public async Task TestGetAllRecipes_RealCalls()
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
        var client = new RecipeClient(mockedLogger.Object, httpClient, config!);

        var recipes = await client.GetAllRecipesAsync(token);
        Assert.That(recipes, Is.Not.Null);
        Assert.That(recipes!.Count, Is.GreaterThanOrEqualTo(415));
    }

    [Test]
    public async Task TestGetAllRecipes_HttpErrors()
    {
        var mockedHttpMessageHandler = new MockHttpMessageHandler();

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<BaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);
        
        var endpoint = $"https://{config.Domain}/recipe/all*";

        var mockedHttpClient = mockedHttpMessageHandler.ToHttpClient();
        mockedHttpMessageHandler.When(endpoint)
                                .Respond(req => new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.NotFound,
                                    Content = new StringContent("Whoops !")
                                });

        var client = new RecipeClient(mockedLogger.Object, mockedHttpClient, config);

        // Should return null reference when Error 500 Internal Server Error is caught
        var recipes = await client.GetAllRecipesAsync(token);
        Assert.That(recipes, Is.Null);

        mockedHttpMessageHandler.When(endpoint)
                                .Respond(req => new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.InternalServerError,
                                    Content = new StringContent("Whoops !")
                                });

        // Now, it should break because the token is missing from current context
        recipes = await client.GetAllRecipesAsync(token);
        Assert.That(recipes, Is.Null);
        
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
                _ = await client.GetAllRecipesAsync(token);
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

    #endregion //! Get All Recipes Tests

    #region GetSingleRecipe By number tests

    [Test]
    public async Task TestGetRecipeByNumber_RealCalls()
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
        var client = new RecipeClient(mockedLogger.Object, httpClient, config!);

        var recipe = await client.GetRecipeByNumberAsync(1, token);
        Assert.That(recipe, Is.Not.Null);
    }
    
    [Test]
    public async Task TestGetRecipeByNumber_HttpErrors()
    {
        var mockedHttpMessageHandler = new MockHttpMessageHandler();

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<BaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);
        
        var endpoint = $"https://{config.Domain}/recipe/bynumber*";
        
        var mockedHttpClient = mockedHttpMessageHandler.ToHttpClient();
        mockedHttpMessageHandler.When(endpoint)
                                .Respond(req => new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.NotFound,
                                    Content = new StringContent("Whoops !")
                                });

        var client = new RecipeClient(mockedLogger.Object, mockedHttpClient, config);

        // Should return null reference when Error 500 Internal Server Error is caught
        var recipe = await client.GetRecipeByNumberAsync(1,token);
        Assert.That(recipe, Is.Null);

        mockedHttpMessageHandler.When(endpoint)
                                .Respond(req => new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.InternalServerError,
                                    Content = new StringContent("Whoops !")
                                });

        // Now, it should break because the token is missing from current context
        recipe = await client.GetRecipeByNumberAsync(1,token);
        Assert.That(recipe, Is.Null);
        
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
                _ = await client.GetRecipeByNumberAsync(1,token);
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
    
    #region Get Recipe By Name
    
    [Test]
    public async Task TestGetRecipeByName_RealCalls()
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
        var client = new RecipeClient(mockedLogger.Object, httpClient, config!);

        var candidateRecipe = await client.GetRecipeByNameAsync("Punk ipa", token);
        Assert.That(candidateRecipe, Is.Not.Null);
    }
    
    [Test]
    public async Task TestGetRecipeByName_HttpErrors()
    {
        var mockedHttpMessageHandler = new MockHttpMessageHandler();

        // Provide a local database handler
        var mockedLogger = new Mock<ILogger<BaseClient>>();
        var config = await TestHelper.GetConfigAsync();

        // Retrieve a real token from execution environment
        var token = TestHelper.GetEnv(TestContstants.AccessTokenEnvVarName);
        
        var endpoint = $"https://{config.Domain}/recipe/byname*";
        
        var mockedHttpClient = mockedHttpMessageHandler.ToHttpClient();
        mockedHttpMessageHandler.When(endpoint)
                                .Respond(req => new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.NotFound,
                                    Content = new StringContent("Whoops !")
                                });

        var client = new RecipeClient(mockedLogger.Object, mockedHttpClient, config);

        // Should return null reference when Error 500 Internal Server Error is caught
        var recipe = await client.GetRecipeByNumberAsync(1,token);
        Assert.That(recipe, Is.Null);

        mockedHttpMessageHandler.When(endpoint)
                                .Respond(req => new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.InternalServerError,
                                    Content = new StringContent("Whoops !")
                                });

        // Now, it should break because the token is missing from current context
        recipe = await client.GetRecipeByNumberAsync(1,token);
        Assert.That(recipe, Is.Null);
        
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
                _ = await client.GetRecipeByNumberAsync(1, token);
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