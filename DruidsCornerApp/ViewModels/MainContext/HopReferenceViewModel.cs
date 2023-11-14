using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DruidsCornerApiClient.Models.RecipeDb;
using DruidsCornerApp.Models.MainContext;
using DruidsCornerApp.Models.References;
using DruidsCornerApp.Services.ResourceProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.HotReload;

namespace DruidsCornerApp.ViewModels.MainContext;

public partial class HopReferenceViewModel : BaseViewModel
{
    private readonly ILogger<HopReferenceViewModel> _logger;
    private readonly HopProvider _hopProvider;
    private int _initialItemCount = 6;
    
    [ObservableProperty]
    private ObservableCollection<CompactHopModel> _hops = new();

    [ObservableProperty]
    private ObservableCollection<string> _hopsNames = new();
    
    [ObservableProperty]
    private HopReferenceFilters _hopFilters = new HopReferenceFilters();
    
    [ObservableProperty]
    private int _remainingItemsThresholdReached = 4;

    /// <summary>
    /// Base reference page (where the whole view is tabbed between hops, yeasts, malts and styles pages)
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="hopProvider"></param>
    public HopReferenceViewModel(ILogger<HopReferenceViewModel> logger,
                                 HopProvider hopProvider
    ) 
    {
        _logger = logger;
        _hopProvider = hopProvider;
        IsLoading = true;
        InitFakeHops();
        IsLoading = false;
    }
    
    private void InitFakeHops()
    {
        Hops.Clear();
        var hops = _hopProvider.GetAllHops();

        // Only load a few items to speed up load time
        for(int i = 0 ; i < _initialItemCount ; i++)
        {
            Hops.Add(CompactHopModelHelper.FromFullModel(hops[i]));
        }
    }

    [RelayCommand]
    public Task RefreshData(CancellationToken cancellationToken)
    {
        IsLoading = true;
        Hops.Clear();
        InitFakeHops();
        IsLoading = false;
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

    /// <summary>
    /// Triggered by the view whenever reaching the bottom of the loading area
    /// It essentially sends the currentCompact Hop where the collection was loaded.
    /// </summary>
    /// <param name="compactHop"></param>
    [RelayCommand]
    public Task LoadMoreHops()
    {
        var hops = _hopProvider.GetAllHops();
        var currentIndex = Hops.Count != 0 ? Hops.Count - 1 : 0;
        if (currentIndex != hops.Count - 1)
        {
            for (int i = currentIndex; i < (currentIndex + RemainingItemsThresholdReached); i++)
            {
                Hops.Add(CompactHopModelHelper.FromFullModel(hops[i]));
            }
            
            // Here this command is called repeatedly.
            // This might be caused by the CollectionView firing it's load more item event, whereas it's being loaded with new item already.
            // So the first event is never completely resolved (?)
        }
        return Task.CompletedTask;
    }
}