using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DruidsCornerApiClient.Services;
using DruidsCornerApp.Models.Login;
using DruidsCornerApp.Models.MainContext;
using DruidsCornerApp.Services;
using DruidsCornerApp.Services.Authentication;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.ViewModels.MainContext;

public partial class HopReferenceViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ISecureStorageService _secureStorageService;
    private readonly ILogger<HopReferenceViewModel> _logger;
    private readonly MainClient _apiClient;

    public ObservableCollection<CompactHopModel> Hops { get; private set; } = new ObservableCollection<CompactHopModel>();

    /// <summary>
    /// Hop reference view model, used alongside the HopReferencePage in order to list the hops remotely
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="authenticationService"></param>
    /// <param name="secureStorageService"></param>
    /// <param name="apiClient"></param>
    public HopReferenceViewModel(ILogger<HopReferenceViewModel> logger,
                                 IAuthenticationService authenticationService,
                                 ISecureStorageService secureStorageService,
                                 MainClient apiClient
    ) : base("Recipes browser", false)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _secureStorageService = secureStorageService;
        _apiClient = apiClient;

        InitData();
    }

    private void InitData()
    {
        Hops = new ObservableCollection<CompactHopModel>()
        {
            new CompactHopModel()
            {
                Name = "Magnum US",
                Purpose = "Bittering",
                Rating = 3.5,
                AlphaAcids = "10 - 16 %",
                Favorite = false,
                StockedAmount = 153
            },
            new CompactHopModel()
            {
                Name = "Magnum GR",
                Purpose = "Bittering",
                Rating = 3.7,
                AlphaAcids = "12 - 16 %",
                Favorite = true,
                StockedAmount = 0
            },
            new CompactHopModel()
            {
                Name = "Cascade",
                Purpose = "Hybrid",
                Rating = 3.8,
                AlphaAcids = "4.5 - 9 %",
                Favorite = true,
                StockedAmount = 86
            },
            new CompactHopModel()
            {
                Name = "Chinook",
                Purpose = "Hybrid",
                Rating = 3.8,
                AlphaAcids = "11.5 - 15 %",
                Favorite = true,
                StockedAmount = 33
            },
            new CompactHopModel()
            {
                Name = "Saaz",
                Purpose = "Aromatic",
                Rating = 3.2,
                AlphaAcids = "2 - 5 %",
                Favorite = false,
                StockedAmount = 0
            },
            new CompactHopModel()
            {
                Name = "Hallertau Blanc",
                Purpose = "Aromatic",
                Rating = 4.1,
                AlphaAcids = "9 - 12 %",
                Favorite = true,
                StockedAmount = 0
            }
        };
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