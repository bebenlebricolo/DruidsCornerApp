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

public class ResourceClient : IResourcesClient
{
    private ILogger<IBaseClient> _logger;
    private readonly HttpClient _httpClient;
    private readonly ClientConfiguration _configuration;
    
    public ResourceClient(ILogger<IBaseClient> logger,
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
}