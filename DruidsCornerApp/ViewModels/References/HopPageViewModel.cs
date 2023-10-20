using DruidsCornerApp.Services;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.ViewModels.References;

public partial class HopPageViewModel : BaseViewModel
{
    private readonly ILogger<HopPageViewModel> _logger;
    private readonly ISecureStorageService _secureStorageService;

    
    /// <summary>
    /// Hop Page view model, displays Hop data
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="secureStorageService"></param>
    public HopPageViewModel(ILogger<HopPageViewModel> logger,
                                ISecureStorageService secureStorageService) : base("Account creation", false)
    {
        _logger = logger;
        _secureStorageService = secureStorageService;
    }
}