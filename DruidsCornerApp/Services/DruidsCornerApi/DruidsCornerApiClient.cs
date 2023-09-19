using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using DruidsCornerApp.Models.Config;
using DruidsCornerApp.Models.DruidsCornerApi.RecipeDb;
using DruidsCornerApp.Models.DruidsCornerApi.Wrappers;
using DruidsCornerApp.Models.Exceptions;
using DruidsCornerApp.Models.Login;
using DruidsCornerApp.Services.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.Services.DruidsCornerApi;

public class DruidsCornerApiClient : IDruidsCornerApiClient
{
    private const string Scheme = "https";
    private const string BearerStr = "Bearer";
    private ILogger<DruidsCornerApiClient> _logger;
    private ISecureStorageService _storageService;
    private readonly HttpClient _httpClient;
    private readonly IConfigProvider _configProvider;
    
    public DruidsCornerApiClient(ILogger<DruidsCornerApiClient> logger,
                                 ISecureStorageService storageService,
                                 HttpClient httpClient,
                                 IConfigProvider configProvider
    )
    {
        _logger = logger;
        _storageService = storageService;
        _httpClient = httpClient;
        _configProvider = configProvider;
    }

    private static JsonSerializerOptions GetJsonOptions()
    {
        return new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
    }
    
    private string GetEndpointUrl(string endpointName, DruidsCornerApiConfig apiConfig)
    {
        var url = $"{Scheme}://{apiConfig.Domain}/recipe/{endpointName}";
        return url;
    }
    
    /// <summary>
    /// Returns a single recipe using its number as a key
    /// </summary>
    /// <param name="number">Recipe's index in database</param>
    /// <returns>Deserialized Recipe, or null in case call issued an error or app is misconfigured</returns>
    /// <exception cref="DruidsCornerApiClientException"></exception>
    public async Task<Recipe?> GetRecipeByNumberAsync(uint number)
    {
        var apiConfig = await _configProvider.GetConfigAsync();
        if (apiConfig == null)
        {
            _logger.LogError("Misconfigured app, cannot proceed.");
            return null;
        }

        var url = GetEndpointUrl("bynumber", apiConfig);
        url += $"?number={number}";

        var token = await _storageService.GetAsync(AccountKeys.TokenKey);
        if (token == null)
        {
            // Should not happen in normal flows
            throw new DruidsCornerApiClientException("Cannot retrieve token from secure storage");
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(BearerStr, token);
        var response = await _httpClient.SendAsync(requestMessage);

        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 400)
        {
            _logger.LogError($"Could not retrieve recipe by number, caught issue within http response");
            _logger.LogError($"StatusCode : {response.StatusCode} ; Reason : {response.ReasonPhrase} ; Headers : {response.Headers}");
            return null;
        }

        try
        {
            var recipe = await JsonSerializer.DeserializeAsync<Recipe>(await response.Content.ReadAsStreamAsync(), GetJsonOptions());
            return recipe;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not retrieve recipe by number, caught issue while deserializing Json response");
            _logger.LogError($"Error was : {ex.Message}");
            return null;
        }
    }

    public async Task<List<Recipe>?> GetAllRecipesAsync()
    {
        var apiConfig = await _configProvider.GetConfigAsync();
        if (apiConfig == null)
        {
            _logger.LogError("Misconfigured app, cannot proceed.");
            return null;
        }

        var url = GetEndpointUrl("all", apiConfig);

        var token = await _storageService.GetAsync(AccountKeys.TokenKey);
        if (token == null)
        {
            // Should not happen in normal flows
            throw new DruidsCornerApiClientException("Cannot retrieve token from secure storage");
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(BearerStr, token);
        var response = await _httpClient.SendAsync(requestMessage);

        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 400)
        {
            _logger.LogError($"Could not retrieve all recipes, caught issue within http response");
            _logger.LogError($"StatusCode : {response.StatusCode} ; Reason : {response.ReasonPhrase} ; Headers : {response.Headers}");
            return null;
        }

        try
        {
            var recipes = await JsonSerializer.DeserializeAsync<List<Recipe>>(await response.Content.ReadAsStreamAsync(), GetJsonOptions());
            return recipes;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not retrieve all recipes list, caught issue while deserializing Json response");
            _logger.LogError($"Error was : {ex.Message}");
            return null;
        }
    }

    public async Task<RecipeResult?> GetRecipeByName(string name)
    {
        var apiConfig = await _configProvider.GetConfigAsync();
        if (apiConfig == null)
        {
            _logger.LogError("Misconfigured app, cannot proceed.");
            return null;
        }

        var url = GetEndpointUrl("byname", apiConfig);
        url += $"?name={name}";

        var token = await _storageService.GetAsync(AccountKeys.TokenKey);
        if (token == null)
        {
            // Should not happen in normal flows
            throw new DruidsCornerApiClientException("Cannot retrieve token from secure storage");
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(BearerStr, token);
        var response = await _httpClient.SendAsync(requestMessage);

        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 400)
        {
            _logger.LogError($"Could not retrieve recipe by name, caught issue within http response");
            _logger.LogError($"StatusCode : {response.StatusCode} ; Reason : {response.ReasonPhrase} ; Headers : {response.Headers}");
            return null;
        }

        try
        {
            var recipe = await JsonSerializer.DeserializeAsync<RecipeResult>(await response.Content.ReadAsStreamAsync(), GetJsonOptions());
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