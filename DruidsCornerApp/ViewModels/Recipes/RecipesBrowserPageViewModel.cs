using CommunityToolkit.Mvvm.Input;
using DruidsCornerAPI.Models.DiyDog.RecipeDb;
using DruidsCornerApp.Services.Authentication;
using DruidsCornerApp.Services.DruidsCornerApi;
using DruidsCornerApp.Utils;
using Firebase.Auth;
using Google.Apis.Requests;

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
        var recipe = await _apiClient.GetRecipeAsync(0);
    }
}