using CommunityToolkit.Mvvm.Input;
using DruidsCornerApp.Models.Login;
using DruidsCornerApp.Services;
using DruidsCornerApp.Utils;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.ViewModels;

public partial class WelcomePageViewModel : BaseViewModel
{
    private readonly ILogger<WelcomePageViewModel> _logger;
    private readonly ISecureStorageService _secureStorageService;

    
    /// <summary>
    /// Welcome Page view model, simply used to bind some commands
    /// and reroute user upon startup 
    /// </summary>
    /// <param name="secureStorageService"></param>
    public WelcomePageViewModel(ILogger<WelcomePageViewModel> logger,
                                        ISecureStorageService secureStorageService) : base("Account creation", false)
    {
        _logger = logger;
        _secureStorageService = secureStorageService;
    }

    [RelayCommand]
    public async Task StartBrowsingClicked(CancellationToken cancellationToken)
    {
        // Already logged in ?
        if (await _secureStorageService.GetAsync(AccountKeys.AccountStateKey) != null)
        {
            // Todo : check if token is still valid
            // if token valid -> go to browsing page
            // else -> go to login page
        }

        // Navigate to basic auth first
        await Shell.Current.GoToAsync($"{Navigator.GetBasicSignInPageRoute()}", true);
    }
}