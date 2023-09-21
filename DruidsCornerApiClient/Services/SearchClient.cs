using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DruidsCornerApiClient.Models;
using DruidsCornerApiClient.Models.Exceptions;
using DruidsCornerApiClient.Models.RecipeDb;
using DruidsCornerApiClient.Models.References.Properties;
using DruidsCornerApiClient.Models.Search;
using DruidsCornerApiClient.Services.Interfaces;
using DruidsCornerApiClient.Utils;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApiClient.Services;

public class SearchClient : BaseClient, ISearchClient
{
    private ILogger<BaseClient> _logger;
    private readonly HttpClient _httpClient;
    private readonly ClientConfiguration _configuration;

    public SearchClient(ILogger<BaseClient> logger,
                        HttpClient httpClient,
                        ClientConfiguration configuration
    )
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
    }

    private string GetEndpointUrl(string endpointName)
    {
        var url = $"{WebConstants.Scheme}://{_configuration.Domain}/search/{endpointName}";
        return url;
    }

    public async Task<List<Recipe>?> SearchAllCandidatesAsync(Queries queries, string token)
    {
        var url = GetEndpointUrl("all");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(WebConstants.BearerStr, token);

        MediaTypeHeaderValue type = new MediaTypeHeaderValue("application/json");
        queries.PreProcessParameters();
        // var queriesJsonStream = new MemoryStream();
        // await JsonSerializer.SerializeAsync(queriesJsonStream, queries, JsonOptionProvider.GetJsonOptions());
        // var streamReader = new StreamReader(queriesJsonStream);
        // queriesJsonStream.Position = 0;
        // var stringRepr = await streamReader.ReadToEndAsync();
        // requestMessage.Content = new StringContent(stringRepr, Encoding.UTF8, type);

        var queriesJsonContent = JsonContent.Create(queries, type ,JsonOptionProvider.GetJsonOptions());
        requestMessage.Content = queriesJsonContent;
        
        var response = await _httpClient.SendAsync(requestMessage);
        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 400)
        {
            HandleResponseStatus(response);

            // Normal error case, when the input query is too restrictive
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogInformation("Could not find a suitable recipe.");
                return null;
            }

            _logger.LogError($"Could not perform search query, caught issue within http response");
            _logger.LogError($"StatusCode : {response.StatusCode} ; Reason : {response.ReasonPhrase} ; Headers : {response.Headers}");
            return null;
        }

        try
        {
            var recipe = await JsonSerializer.DeserializeAsync<List<Recipe>>(await response.Content.ReadAsStreamAsync(),
                                                                             JsonOptionProvider.GetJsonOptions());
            return recipe;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not perform search query, caught issue while deserializing Json response");
            _logger.LogError($"Error was : {ex.Message}");
            return null;
        }
    }

    private string EncodeNamesQuery(List<string> names)
    {
        var encodedStr = "";
        // Encodes like that : <url>?names=item1&names=item2
        for (int i = 0; i < names.Count; i++)
        {
            encodedStr += names[i];
            if (i != names.Count - 1)
            {
                encodedStr += "&names=";
            }
        }

        return encodedStr;
    }

    public async Task<List<HopProperty>?> SearchHopsByNameAsync(List<string> names, string token)
    {
        var url = GetEndpointUrl("hops");
        url += "?names=";
        url += EncodeNamesQuery(names);

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(WebConstants.BearerStr, token);
        var response = await _httpClient.SendAsync(requestMessage);
        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 400)
        {
            HandleResponseStatus(response);

            _logger.LogError($"Could not perform hops search query, caught issue within http response");
            _logger.LogError($"StatusCode : {response.StatusCode} ; Reason : {response.ReasonPhrase} ; Headers : {response.Headers}");
            return null;
        }

        try
        {
            var hopProperties = await JsonSerializer.DeserializeAsync<List<HopProperty>>(await response.Content.ReadAsStreamAsync(),
                                                                                         JsonOptionProvider.GetJsonOptions());
            return hopProperties;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not perform hops search query, caught issue while deserializing Json response");
            _logger.LogError($"Error was : {ex.Message}");
            return null;
        }
    }

    public async Task<List<MaltProperty>?> SearchMaltsByNameAsync(List<string> names, string token)
    {
        var url = GetEndpointUrl("malts");
        url += "?names=";
        url += EncodeNamesQuery(names);

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(WebConstants.BearerStr, token);
        var response = await _httpClient.SendAsync(requestMessage);
        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 400)
        {
            HandleResponseStatus(response);

            _logger.LogError($"Could not perform malts search query, caught issue within http response");
            _logger.LogError($"StatusCode : {response.StatusCode} ; Reason : {response.ReasonPhrase} ; Headers : {response.Headers}");
            return null;
        }

        try
        {
            var maltProperties = await JsonSerializer.DeserializeAsync<List<MaltProperty>>(await response.Content.ReadAsStreamAsync(),
                                                                                           JsonOptionProvider.GetJsonOptions());
            return maltProperties;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not perform malts search query, caught issue while deserializing Json response");
            _logger.LogError($"Error was : {ex.Message}");
            return null;
        }
    }

    public async Task<List<YeastProperty>?> SearchYeastsByNameAsync(List<string> names, string token)
    {
        var url = GetEndpointUrl("yeasts");
        url += "?names=";
        url += EncodeNamesQuery(names);

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(WebConstants.BearerStr, token);
        var response = await _httpClient.SendAsync(requestMessage);
        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 400)
        {
            HandleResponseStatus(response);

            _logger.LogError($"Could not perform yeasts search query, caught issue within http response");
            _logger.LogError($"StatusCode : {response.StatusCode} ; Reason : {response.ReasonPhrase} ; Headers : {response.Headers}");
            return null;
        }

        try
        {
            var yeastProperties = await JsonSerializer.DeserializeAsync<List<YeastProperty>>(await response.Content.ReadAsStreamAsync(),
                                                                                             JsonOptionProvider.GetJsonOptions());
            return yeastProperties;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not perform yeasts search query, caught issue while deserializing Json response");
            _logger.LogError($"Error was : {ex.Message}");
            return null;
        }
    }

    public async Task<List<StyleProperty>?> SearchStylesByNameAsync(List<string> names, string token, uint minimumMatchingScore = 50)
    {
        var url = GetEndpointUrl("styles");
        url += "?names=";
        url += EncodeNamesQuery(names);

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(WebConstants.BearerStr, token);
        var response = await _httpClient.SendAsync(requestMessage);
        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 400)
        {
            HandleResponseStatus(response);

            _logger.LogError($"Could not perform styles search query, caught issue within http response");
            _logger.LogError($"StatusCode : {response.StatusCode} ; Reason : {response.ReasonPhrase} ; Headers : {response.Headers}");
            return null;
        }

        try
        {
            var stylesProperties = await JsonSerializer.DeserializeAsync<List<StyleProperty>>(await response.Content.ReadAsStreamAsync(),
                                                                                              JsonOptionProvider.GetJsonOptions());
            return stylesProperties;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not perform styles search query, caught issue while deserializing Json response");
            _logger.LogError($"Error was : {ex.Message}");
            return null;
        }
    }
}