using DruidsCornerApp.ViewModels.MainContext;

namespace DruidsCornerApp.Views.MainContext;

public partial class ReferencesPage : ContentPage
{
    private readonly ReferencesPageViewModel _viewModel;
    
    public ReferencesPage(ReferencesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Switcher.SelectedIndex = -1;
        Switcher.SelectedIndex = _viewModel.SelectedViewModelIndex;
    }
}