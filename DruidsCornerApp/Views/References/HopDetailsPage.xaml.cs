using CommunityToolkit.Mvvm.ComponentModel;
using DruidsCornerApp.Models.References;
using DruidsCornerApp.Services.ResourceProviders;
using DruidsCornerApp.ViewModels.References;

namespace DruidsCornerApp.Views.References;

public partial class HopDetailsPage : ContentPage, IQueryAttributable
{
    private readonly HopPageViewModel _viewModel;
    private readonly HopProvider _hopProvider;
    
    public HopDetailsPage(HopPageViewModel viewModel,
                   HopProvider hopProvider)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        _hopProvider = hopProvider;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = query["id"] as string;
        _viewModel.LoadHopFromId(id!);
    }
}