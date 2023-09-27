using CommunityToolkit.Mvvm.Input;
using DruidsCornerApp.Models.Login;
using DruidsCornerApp.Services;
using DruidsCornerApp.Utils;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.ViewModels;

public partial class DefaultPageViewModel : BaseViewModel
{
    private readonly ILogger<DefaultPageViewModel> _logger;
    private readonly ISecureStorageService _secureStorageService;

    
    /// <summary>
    /// Welcome Page view model, simply used to bind some commands
    /// and reroute user upon startup 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="secureStorageService"></param>
    public DefaultPageViewModel(ILogger<DefaultPageViewModel> logger,
                                ISecureStorageService secureStorageService) : base("Account creation", false)
    {
        _logger = logger;
        _secureStorageService = secureStorageService;
    }
}