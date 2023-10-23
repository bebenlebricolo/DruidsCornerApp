using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DruidsCornerApp.Models.MainContext;
using DruidsCornerApp.Models.References;
using DruidsCornerApp.Services.ResourceProviders;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.ViewModels.MainContext;

public partial class ReferencesPageViewModel : ObservableObject
{
    private readonly ILogger<RecipeExplorerViewModel> _logger;
    private readonly HopProvider _hopProvider;

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
    /// <param name="hopProvider"></param>
    public ReferencesPageViewModel(ILogger<RecipeExplorerViewModel> logger,
                                   HopProvider hopProvider
    )
    {
        _logger = logger;
        _hopProvider = hopProvider;
        InitFakeHops();
    }
    
    private void InitFakeHops()
    {
        Hops.Clear();
        var hops = _hopProvider.GetAllHops();

        foreach (var hop in hops)
        {
            Hops.Add(CompactHopModelHelper.FromFullModel(hop));
        }
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
    public async Task HopCardClicked(CompactHopModel hopModel)
    {
        await Shell.Current.GoToAsync($"HopPage?id={hopModel.Id}");
    }

    [RelayCommand]
    public Task HopToggleFavorite(CompactHopModel hopModel)
    {
        hopModel.Favorite = !hopModel.Favorite;
        
        return Task.CompletedTask;
    }
}