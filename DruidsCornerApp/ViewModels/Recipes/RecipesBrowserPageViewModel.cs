using CommunityToolkit.Mvvm.Input;
using DruidsCornerApiClient.Models.RecipeDb;
using DruidsCornerApiClient.Services;
using DruidsCornerApp.Models.Login;
using DruidsCornerApp.Services.Authentication;

namespace DruidsCornerApp.ViewModels.Recipes;
using CommunityToolkit.Mvvm.ComponentModel;
using DruidsCornerApp.Services;
using Microsoft.Extensions.Logging;


public partial class RecipesBrowserPageViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ISecureStorageService _secureStorageService;
    private readonly ILogger<RecipesBrowserPageViewModel> _logger;
    private readonly IDruidsCornerApiClient _apiClient;
    
    private List<Recipe> recipes;

    /// <summary>
    /// User email
    /// </summary>
    [ObservableProperty]
    private string _email = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="authenticationService"></param>
    /// <param name="secureStorageService"></param>
    public RecipesBrowserPageViewModel(ILogger<RecipesBrowserPageViewModel> logger,
                                       IAuthenticationService authenticationService,
                                       ISecureStorageService secureStorageService,
                                       IDruidsCornerApiClient apiClient) : base("Recipes browser", false)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _secureStorageService = secureStorageService;
        _apiClient = apiClient;
    }


    [RelayCommand]
    public async Task RefreshContent()
    {
        // Fetch new content
        var token = await _secureStorageService.GetAsync(AccountKeys.TokenKey);
        if (token == null)
        {
            throw new UnauthorizedAccessException("Access token should be present to use this api client !");
        }
        var recipe = await _apiClient.GetRecipeByNumberAsync(0, token);
    }
}