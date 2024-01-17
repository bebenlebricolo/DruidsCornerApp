using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DruidsCornerApiClient.Models.RecipeDb;
using DruidsCornerApp.Models.MainContext;
using DruidsCornerApp.Models.References;
using DruidsCornerApp.Services.ResourceProviders;
using DruidsCornerApp.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.HotReload;

namespace DruidsCornerApp.ViewModels.MainContext;

public partial class HopReferenceViewModel : BaseViewModel
{
    private readonly ILogger<HopReferenceViewModel> _logger;
    private readonly HopProvider _hopProvider;
    private int _initialItemCount = 6;
    
    [ObservableProperty]
    private BatchObservableCollection<CompactHopModel> _hops = new();

    [ObservableProperty]
    private BatchObservableCollection<string> _hopsNames = new();
    
    [ObservableProperty]
    private HopReferenceFilters _hopFilters = new HopReferenceFilters();
    
    [ObservableProperty]
    private int _remainingItemsThresholdReached = 4;

    [ObservableProperty]
    private bool _goUpPageButtonVisible = false;
    
    /// <summary>
    /// Amount of new items to be loaded whenever the end of the
    /// collection is reached. A higher number will expand the collection view more than the other one.
    /// </summary>
    [ObservableProperty]
    private int _loadItemCount = 15;
    
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

        var defaultList = new List<CompactHopModel>();
        // Only load a few items to speed up load time
        for(int i = 0 ; i < _initialItemCount ; i++)
        {
            defaultList.Add(CompactHopModelHelper.FromFullModel(hops[i]));
        }

        Hops.InsertRange(defaultList);
    }

    [RelayCommand]
    public async Task RefreshData(CancellationToken cancellationToken)
    {
        IsLoading = true;
        Hops.Clear();
        InitFakeHops();
        IsLoading = false;
        await Task.CompletedTask;
    }

    [RelayCommand]
    public async Task AddOneHop(CancellationToken cancellationToken)
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
        await Task.CompletedTask;
    }

    [RelayCommand]
    public async Task AddHopNameFilterLabel(Entry entryView, CancellationToken cancellationToken)
    {
        if (!HopsNames.Contains(entryView.Text) && !string.IsNullOrEmpty(entryView.Text))
        {
            HopsNames.Add(entryView.Text);
            HopFilters.Names.Add(entryView.Text);
        }

        entryView.Text = "";
        await Task.CompletedTask;
    }

    [RelayCommand]
    public async Task RemoveHopNameFilterLabel(string hopName)
    {
        if (HopsNames.Contains(hopName))
        {
            HopsNames.Remove(hopName);
            HopFilters.Names.Remove(hopName);
        }

        await Task.CompletedTask;
    }

    [RelayCommand]
    public async Task HopCardClicked(CompactHopModel hopModel)
    {
        await Shell.Current.GoToAsync($"HopPage?id={hopModel.Id}", true);
    }

    [RelayCommand]
    public async Task HopToggleFavorite(CompactHopModel hopModel)
    {
        hopModel.Favorite = !hopModel.Favorite;
        
        await Task.CompletedTask;
    }

    /// <summary>
    /// Triggered by the view whenever reaching the bottom of the loading area
    /// It essentially sends the currentCompact Hop where the collection was loaded.
    /// </summary>
    [RelayCommand]
    public async Task LoadMoreHopsAsync()
    {
        var totalHops = _hopProvider.GetAllHops();
        var currentIndex = Hops.Count != 0 ? Hops.Count - 1 : 0;
        if (currentIndex < totalHops.Count - 1 )
        {
            var newHopList = new List<CompactHopModel>();
            for (int i = currentIndex; i < (currentIndex + LoadItemCount) && (i < totalHops.Count - 1); i++)
            {
                newHopList.Add(CompactHopModelHelper.FromFullModel(totalHops[i]));
            }

            // Here this command is called repeatedly.
            // This might be caused by the CollectionView firing it's load more item event, whereas it's being loaded with new item already.
            // So the first event is never completely resolved (?)
            
            // if RecyclerView is still updating, wait before inserting new data
            // otherwise it breaks apart (OnNotifyChanged should not be called when RecyclerView is scrolling or computing layout)
            Hops.InsertRange(newHopList);
        }
        await Task.CompletedTask;
    }
}