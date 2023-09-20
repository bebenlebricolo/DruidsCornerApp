using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using DruidsCornerApiClient.Models;
using DruidsCornerApiClient.Models.Exceptions;
using DruidsCornerApiClient.Models.RecipeDb;
using DruidsCornerApiClient.Models.Wrappers;
using DruidsCornerApiClient.Services.Interfaces;
using DruidsCornerApiClient.Utils;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApiClient.Services;

public class RecipeClient : IRecipeClient
{
    private ILogger<IBaseClient> _logger;
    private readonly HttpClient _httpClient;
    private readonly ClientConfiguration _configuration;
    
    public RecipeClient(ILogger<IBaseClient> logger,
                  HttpClient httpClient,
                  ClientConfiguration configuration  )
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
    }

    private string GetRecipeEndpointUrl(string endpointName)
    {
        var url = $"{WebConstants.Scheme}://{_configuration.Domain}/recipe/{endpointName}";
        return url;
    }
    
    /// <summary>
    /// Returns a single recipe using its number as a key
    /// </summary>
    /// <param name="number">Recipe's index in database</param>
    /// <returns>Deserialized Recipe, or null in case call issued an error or app is misconfigured</returns>
    /// <exception cref="DruidsCornerApiClientException"></exception>
    public async Task<Recipe?> GetRecipeByNumberAsync(uint number, string token)
    {
        var url = GetRecipeEndpointUrl("bynumber");
        url += $"?number={number}";

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(WebConstants.BearerStr, token);
        var response = await _httpClient.SendAsync(requestMessage);

        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 400)
        {
            _logger.LogError($"Could not retrieve recipe by number, caught issue within http response");
            _logger.LogError($"StatusCode : {response.StatusCode} ; Reason : {response.ReasonPhrase} ; Headers : {response.Headers}");
            return null;
        }

        try
        {
            var recipe = await JsonSerializer.DeserializeAsync<Recipe>(await response.Content.ReadAsStreamAsync(), JsonOptionProvider.GetJsonOptions());
            return recipe;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not retrieve recipe by number, caught issue while deserializing Json response");
            _logger.LogError($"Error was : {ex.Message}");
            return null;
        }
    }

    public async Task<List<Recipe>?> GetAllRecipesAsync(string token)
    {
        var url = GetRecipeEndpointUrl("all");

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(WebConstants.BearerStr, token);
        var response = await _httpClient.SendAsync(requestMessage);

        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 400)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                // Throwing an authentication failure will tell upper layers to try to reauthenticate with the servers.
                throw new ClientException($"Caught authentication issue. Status code was : {response.StatusCode}",
                                                         FailureModes.AuthenticationFailure);
            }
            _logger.LogError($"Could not retrieve all recipes, caught issue within http response");
            _logger.LogError($"StatusCode : {response.StatusCode} ; Reason : {response.ReasonPhrase} ; Headers : {response.Headers}");
            return null;
        }

        try
        {
            var recipes = await JsonSerializer.DeserializeAsync<List<Recipe>>(await response.Content.ReadAsStreamAsync(), JsonOptionProvider.GetJsonOptions());
            return recipes;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not retrieve all recipes list, caught issue while deserializing Json response");
            _logger.LogError($"Error was : {ex.Message}");
            return null;
        }
    }

    public async Task<RecipeResult?> GetRecipeByNameAsync(string name, string token)
    {
        var url = GetRecipeEndpointUrl("byname");
        url += $"?name={name}";

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(WebConstants.BearerStr, token);
        var response = await _httpClient.SendAsync(requestMessage);

        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 400)
        {
            _logger.LogError($"Could not retrieve recipe by name, caught issue within http response");
            _logger.LogError($"StatusCode : {response.StatusCode} ; Reason : {response.ReasonPhrase} ; Headers : {response.Headers}");
            return null;
        }

        try
        {
            var recipe = await JsonSerializer.DeserializeAsync<RecipeResult>(await response.Content.ReadAsStreamAsync(), JsonOptionProvider.GetJsonOptions());
            return recipe;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not retrieve recipe by name, caught issue while deserializing Json response");
            _logger.LogError($"Error was : {ex.Message}");
            return null;
        }
    }
}