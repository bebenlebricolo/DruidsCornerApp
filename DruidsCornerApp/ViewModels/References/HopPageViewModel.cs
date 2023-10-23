using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using DruidsCornerApiClient.Utils;
using DruidsCornerApp.Models.MainContext;
using DruidsCornerApp.Models.References;
using DruidsCornerApp.Services;
using DruidsCornerApp.Services.ResourceProviders;
using DruidsCornerApp.Services.StaticData;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.ViewModels.References;

public partial class HopPageViewModel : ObservableObject
{
    private readonly ILogger<HopPageViewModel> _logger;
    private readonly ISecureStorageService _secureStorageService;
    private readonly HopProvider _hopProvider;

    [ObservableProperty]
    private HopModel _hopModel = new();
    
    /// <summary>
    /// Hop Page view model, displays Hop data
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="secureStorageService"></param>
    public HopPageViewModel(ILogger<HopPageViewModel> logger,
                            ISecureStorageService secureStorageService,
                            HopProvider hopProvider)
    {
        _logger = logger;
        _secureStorageService = secureStorageService;
        _hopProvider = hopProvider;
    }

    public void LoadHopFromId(string id)
    {
        var target = _hopProvider.GetFromId(id);
        if (target != null)
        {
            HopModel = target;
        }
    }
}