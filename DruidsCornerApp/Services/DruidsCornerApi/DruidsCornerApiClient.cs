using System.Net.Http.Headers;
using DruidsCornerAPI.Models.DiyDog.RecipeDb;
using DruidsCornerApp.Models.Config;
using DruidsCornerApp.Models.Exceptions;
using DruidsCornerApp.Models.Login;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.Services.DruidsCornerApi;

public class DruidsCornerApiClient : IDruidsCornerApiClient
{
    private ILogger<DruidsCornerApiClient> _logger;
    private IConfiguration _configuration;
    private ISecureStorageService _storageService;
    private readonly HttpClient _httpClient;

    public DruidsCornerApiClient(ILogger<DruidsCornerApiClient> logger,
                                 IConfiguration configuration,
                                 ISecureStorageService storageService,
                                 HttpClient httpClient
    )
    {
        _logger = logger;
        _configuration = configuration;
        _storageService = storageService;
        _httpClient = httpClient;
    }

    public async Task<Recipe> GetRecipeAsync(int number)
    {
        var apiConfig = _configuration.GetValue<DruidsCornerApiConfig>(DruidsCornerApiConfig.SectionName);
        var url = $"http://{apiConfig.Domain}/recipe/all";

        var token = await _storageService.GetAsync(AccountKeys.TokenKey);
        if (token == null)
        {
            // Should not happen in normal flows
            throw new DruidsCornerApiClientException("Cannot retrieve token from secure storage");
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(requestMessage);
        
        
        
        
        return new Recipe();
    }

    public async Task<List<Recipe>> GetAllRecipesAsync()
    {
        return new List<Recipe>();
    }

}