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

public partial class ReferencesPageViewModel : BaseViewModel
{
    private readonly ILogger<RecipeExplorerViewModel> _logger;

    [ObservableProperty]
    private int _selectedViewModelIndex = 0;

    /// <summary>
    /// Base reference page (where the whole view is tabbed between hops, yeasts, malts and styles pages)
    /// </summary>
    /// <param name="logger"></param>
    public ReferencesPageViewModel(ILogger<RecipeExplorerViewModel> logger
    ) : base("Recipes browser", false)
    {
        _logger = logger;
    }
}