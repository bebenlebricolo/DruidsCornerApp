using System.Net;
using DruidsCornerAPI.Models.DiyDog.RecipeDb;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.Services.DruidsCornerApi;

public class DruidsCornerApiClient : IDruidsCornerApiClient
{
    private ILogger<DruidsCornerApiClient> _logger;
    private readonly HttpClient _httpClient;

    public DruidsCornerApiClient(ILogger<DruidsCornerApiClient> logger,
                                 HttpClient httpClient
    )
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public Task<Recipe> GetRecipeAsync(int number)
    {
        throw new NotImplementedException();
    }
}