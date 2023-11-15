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
    private readonly ILogger<ReferencesPageViewModel> _logger;

    public HopReferenceViewModel HopReferenceViewModel { get; }

    [ObservableProperty]
    private int _selectedViewModelIndex = 0;
    
    /// <summary>
    /// Base reference page (where the whole view is tabbed between hops, yeasts, malts and styles pages)
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="hopReferenceViewModel"></param>
    public ReferencesPageViewModel(ILogger<ReferencesPageViewModel> logger,
                                   HopReferenceViewModel hopReferenceViewModel
    )
    {
        _logger = logger;
        HopReferenceViewModel = hopReferenceViewModel;
    }
}