using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using DruidsCornerApp.Models.MainContext;
using DruidsCornerApp.Services;
using DruidsCornerApp.ViewModels.MainContext;

namespace DruidsCornerApp.Views.MainContext;

/// <summary>
/// Hop reference page ; view model is linked to ReferencePageViewModel, as this component is "just" a view.
/// </summary>
public partial class HopReferenceView : ContentView
{
    private readonly ImageSource _heartSource = ImageSource.FromFile("heart.svg");
    private readonly ImageSource _heartFullSource = ImageSource.FromFile("heart_full.svg");

    public HopReferenceView()
    {
        InitializeComponent();
        HopCardsCollectionView.Scrolled += CollectionViewScrolled;
    }

    private void CollectionViewScrolled(object? sender, EventArgs args)
    {
        // Its a bit weird to forward control to the view model that way, I hope it does not break the whole thing apart (...)
        // Seems very fragile though !        
        ItemsViewScrolledEventArgs castArgs = (ItemsViewScrolledEventArgs) args;
        var collectionViewScrolled = castArgs.FirstVisibleItemIndex != 0;
        (BindingContext as ReferencesPageViewModel)!.HopReferenceViewModel.GoUpPageButtonVisible =  collectionViewScrolled; 
    }
    
    private void GoUpButtonClicked(object? sender, EventArgs e)
    {
        // Go to origin of view
        HopCardsCollectionView.ScrollTo(0);
    }


    private void HopNameTextEntry_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        // Only show the "+" button when user is typing some hop names
        if (!string.IsNullOrEmpty(HopNameTextEntry.Text))
        {
            HopNameEntryPlusButton.IsVisible = true;
        }
        else
        {
            HopNameEntryPlusButton.IsVisible = false;
        }
    }

    private void HopCardsCollectionView_OnRemainingItemsThresholdReached(object? sender, EventArgs e)
    {
        var model = BindingContext as ReferencesPageViewModel;
        //App.Current.Dispatcher.DispatchAsync(async () => model!.HopReferenceViewModel.LoadMoreHopsAsync());
        Task.Run(async () => await model!.HopReferenceViewModel.LoadMoreHopsAsync());
    }
}