using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DruidsCornerApiClient.Services;
using DruidsCornerApp.Models.Login;
using DruidsCornerApp.Models.RecipeExplorer;
using DruidsCornerApp.Services;
using DruidsCornerApp.Services.Authentication;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.ViewModels.MainContext;

public partial class RecipeExplorerViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ISecureStorageService _secureStorageService;
    private readonly ILogger<RecipeExplorerViewModel> _logger;
    private readonly MainClient _apiClient;

    public ObservableCollection<CompactRecipe> Recipes { get; } = new ObservableCollection<CompactRecipe>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="authenticationService"></param>
    /// <param name="secureStorageService"></param>
    /// <param name="apiClient"></param>
    public RecipeExplorerViewModel(ILogger<RecipeExplorerViewModel> logger,
                                   IAuthenticationService authenticationService,
                                   ISecureStorageService secureStorageService,
                                   MainClient apiClient
    ) : base("Recipes browser", false)
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
            Abv = 3.5,
            Ebc = 20,
            Ibu = 35,
            BrewDate = "2018",
            Favorite = true,
            Rating = 4.5,
            ImageSource =
                "https://www.brewdog.com/media/catalog/product/cache/eb360c13587b21a8ac6c611a2762b864/b/d/bd_webbundle_pdp_1x330can_single_punk.png"
        });
        Recipes.Add(new CompactRecipe()
        {
            Brewer = "BrewDog",
            Name = "Speed Bird ",
            Style = "Transatlantic Ipa",
            Abv = 3.5,
            Ebc = 20,
            Ibu = 35,
            BrewDate = "2018",
            Favorite = true,
            Rating = 3,
            ImageSource = "https://brewdog.mtchost.com/public/files/BLOG%20PHOTOS/Speedbird100.jpg"
        });
        Recipes.Add(new CompactRecipe()
        {
            Brewer = "BrewDog",
            Name = "Chaos Theory",
            Style = "Predictably Random Ipa",
            Abv = 3.5,
            Ebc = 20,
            Ibu = 35,
            BrewDate = "2018",
            Favorite = false,
            Rating = 3.1,
            ImageSource = "https://brewdogrecipes.com/assets/recipes/19.jpg"
        });
        Recipes.Add(new CompactRecipe()
        {
            Brewer = "BrewDog",
            Name = "Tokyo Rising Sun - Lowland",
            Style = "A Beautiful Accident",
            Abv = 3.5,
            Ebc = 20,
            Ibu = 35,
            BrewDate = "2018",
            Favorite = true,
            Rating = 4.2,
            ImageSource = "https://brewdogrecipes.com/assets/recipes/27.jpg"
        });
        Recipes.Add(new CompactRecipe()
        {
            Brewer = "BrewDog",
            Name = "Sink the Bismarck !",
            Style = "IPA For the Dedicated",
            Abv = 3.5,
            Ebc = 20,
            Ibu = 35,
            BrewDate = "2018",
            Favorite = false,
            Rating = 3.5,
            ImageSource = "https://brewdogrecipes.com/assets/recipes/27.jpg"
        });
        Recipes.Add(new CompactRecipe()
        {
            Brewer = "BrewDog",
            Name = "Hardkogt Ipa",
            Style = "A SPECIAL DOUBLE IPA FOR OUR DANISH FRIENDS",
            Abv = 3.5,
            Ebc = 20,
            Ibu = 35,
            BrewDate = "2018",
            Favorite = false,
            Rating = 3.6,
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