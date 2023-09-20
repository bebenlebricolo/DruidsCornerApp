using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using DruidsCornerApiClient.Models;
using DruidsCornerApiClient.Models.Exceptions;
using DruidsCornerApiClient.Models.RecipeDb;
using DruidsCornerApiClient.Models.References.Properties;
using DruidsCornerApiClient.Models.Search;
using DruidsCornerApiClient.Models.Wrappers;
using DruidsCornerApiClient.Services.Interfaces;
using DruidsCornerApiClient.Utils;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApiClient.Services;

public class SearchClient : ISearchClient
{
    private ILogger<IBaseClient> _logger;
    private readonly HttpClient _httpClient;
    private readonly ClientConfiguration _configuration;
    
    public SearchClient(ILogger<IBaseClient> logger,
                        HttpClient httpClient,
                        ClientConfiguration configuration  )
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
    }
    
    private string GetRecipeEndpointUrl(string endpointName)
    {
        var url = $"{WebConstants.Scheme}://{_configuration.Domain}/search/{endpointName}";
        return url;
    }

    public async Task<List<Recipe>> SearchAllCandidatesAsync(Queries queries, string token)
    {
            throw new NotImplementedException();
    }

    public async Task<List<HopProperty>> SearchHopsByNameAsync(List<string> names, string token)
    {
        throw new NotImplementedException();
    }

    public async Task<List<MaltProperty>> SearchMaltsByNameAsync(List<string> names, string token)
    {
        throw new NotImplementedException();
    }

    public async Task<List<YeastProperty>> SearchYeastsByNameAsync(List<string> names, string token)
    {
        throw new NotImplementedException();
    }

    public async Task<List<StyleProperty>> SearchStylesByNameAsync(List<string> names, string token, uint minimumMatchingScore = 50)
    {
        throw new NotImplementedException();
    }
}