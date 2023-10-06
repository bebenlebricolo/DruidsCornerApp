using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DruidsCornerApiClient.Models.RecipeDb;
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
    private ObservableCollection<CompactHopModel> _hops = new();

    [ObservableProperty]
    private ObservableCollection<string> _hopsNames = new();
    
    [ObservableProperty]
    private HopReferenceFilters _hopFilters = new HopReferenceFilters();
    
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
        InitFakeHops();
    }
    
    private void InitFakeHops()
    {
        Hops.Clear();
        Hops.Add(new CompactHopModel
        {
            Name = "Magnum US",
            Purpose = "Bittering",
            Rating = 3.5,
            AlphaAcids = "10 - 16 %",
            Favorite = false,
            StockedAmount = 153
        });
        Hops.Add(new CompactHopModel
        {
            Name = "Magnum GR",
            Purpose = "Bittering",
            Rating = 3.7,
            AlphaAcids = "12 - 16 %",
            Favorite = true,
            StockedAmount = 0
        });
        Hops.Add(new CompactHopModel
        {
            Name = "Cascade",
            Purpose = "Hybrid",
            Rating = 3.8,
            AlphaAcids = "4.5 - 9 %",
            Favorite = true,
            StockedAmount = 86
        });

        Hops.Add(new CompactHopModel
        {
            Name = "Chinook",
            Purpose = "Hybrid",
            Rating = 3.8,
            AlphaAcids = "11.5 - 15 %",
            Favorite = true,
            StockedAmount = 33
        });

        Hops.Add(new CompactHopModel
        {
            Name = "Saaz",
            Purpose = "Aromatic",
            Rating = 3.2,
            AlphaAcids = "2 - 5 %",
            Favorite = false,
            StockedAmount = 0
        });

        Hops.Add(new CompactHopModel
        {
            Name = "Hallertau Blanc",
            Purpose = "Aromatic",
            Rating = 4.1,
            AlphaAcids = "9 - 12 %",
            Favorite = true,
            StockedAmount = 0
        });
    }

    [RelayCommand]
    public Task RefreshData(CancellationToken cancellationToken)
    {
        InitFakeHops();
        return Task.CompletedTask;
    }

    [RelayCommand]
    public Task AddOneHop(CancellationToken cancellationToken)
    {
        Hops.Add(new CompactHopModel()
        {
            Name = "Fake hop!",
            Purpose = "Don't know",
            Rating = 4.1,
            AlphaAcids = "9 - 12 %",
            Favorite = true,
            StockedAmount = 0
        });
        return Task.CompletedTask;
    }

    [RelayCommand]
    public Task AddHopNameFilterLabel(Entry entryView, CancellationToken cancellationToken)
    {
        if (!HopsNames.Contains(entryView.Text) && !string.IsNullOrEmpty(entryView.Text))
        {
            HopsNames.Add(entryView.Text);
            HopFilters.Names.Add(entryView.Text);
        }

        entryView.Text = "";
        return Task.CompletedTask;
    }

    [RelayCommand]
    public Task RemoveHopNameFilterLabel(string hopName)
    {
        if (HopsNames.Contains(hopName))
        {
            HopsNames.Remove(hopName);
            HopFilters.Names.Remove(hopName);
        }
        return Task.CompletedTask;
    }

    [RelayCommand]
    public Task HopCardClicked(CompactHopModel hopModel)
    {
        return Task.CompletedTask;
    }

    [RelayCommand]
    public Task HopToggleFavorite(CompactHopModel hopModel)
    {
        hopModel.Favorite = !hopModel.Favorite;
        
        return Task.CompletedTask;
    }
}