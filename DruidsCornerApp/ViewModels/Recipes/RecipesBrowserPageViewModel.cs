using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using DruidsCornerApiClient.Models.RecipeDb;
using DruidsCornerApiClient.Services;
using DruidsCornerApiClient.Services.Interfaces;
using DruidsCornerApp.Models.Login;
using DruidsCornerApp.Models.RecipeExplorer;
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
    private readonly MainClient _apiClient;
    
    public ObservableCollection<CompactRecipe> Recipes { get; } = new ObservableCollection<CompactRecipe>();

    /// <summary>
    /// User email
    /// </summary>
    [ObservableProperty]
    private string _email = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="authenticationService"></param>
    /// <param name="secureStorageService"></param>
    /// <param name="apiClient"></param>
    public RecipesBrowserPageViewModel(ILogger<RecipesBrowserPageViewModel> logger,
                                       IAuthenticationService authenticationService,
                                       ISecureStorageService secureStorageService,
                                       MainClient apiClient) : base("Recipes browser", false)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _secureStorageService = secureStorageService;
        _apiClient = apiClient;
        
        InitRecipesList();
    }

    private void InitRecipesList()
    {
        Recipes.Add(new CompactRecipe()
        {
            Brewer = "BrewDog",
            Name = "Punk Ipa",
            Style = "India Pale Ale",
            ImageSource =
                "https://www.brewdog.com/media/catalog/product/cache/eb360c13587b21a8ac6c611a2762b864/b/d/bd_webbundle_pdp_1x330can_single_punk.png"
        });
        Recipes.Add(new CompactRecipe()
        {
            Brewer = "BrewDog",
            Name = "Speed Bird ",
            Style = "Transatlantic Ipa",
            ImageSource = "https://brewdog.mtchost.com/public/files/BLOG%20PHOTOS/Speedbird100.jpg"
        });
        Recipes.Add(new CompactRecipe()
        {
            Brewer = "BrewDog",
            Name = "Chaos Theory",
            Style = "Predictably Random Ipa",
            ImageSource = "https://brewdogrecipes.com/assets/recipes/19.jpg"
        });
        Recipes.Add(new CompactRecipe()
        {
            Brewer = "BrewDog",
            Name = "Tokyo Rising Sun - Lowland",
            Style = "A Beautiful Accident",
            ImageSource = "https://brewdogrecipes.com/assets/recipes/27.jpg"
        });
        Recipes.Add(new CompactRecipe()
        {
            Brewer = "BrewDog",
            Name = "Sink the Bismarck !",
            Style = "IPA For the Dedicated",
            ImageSource = "https://brewdogrecipes.com/assets/recipes/27.jpg"
        });
        Recipes.Add(new CompactRecipe()
        {
            Brewer = "BrewDog",
            Name = "Hardkogt Ipa",
            Style = "A SPECIAL DOUBLE IPA FOR OUR DANISH FRIENDS",
            ImageSource = "https://brewdogrecipes.com/assets/recipes/46.jpg"
        });
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
        var recipe = await _apiClient.Recipe!.GetRecipeByNumberAsync(2, token);
    }
}