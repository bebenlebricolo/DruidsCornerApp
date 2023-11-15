using CommunityToolkit.Mvvm.ComponentModel;
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
        // Its a bit weird to forward control to the view model that way, I hope it does not break the whole thing appart (...)
        // Seems very fragile though !        
        ItemsViewScrolledEventArgs castArgs = (ItemsViewScrolledEventArgs) args;
        var topScrollViewScrolled = TopScrollView.ScrollY > 0;
        var collectionViewScrolled = castArgs.FirstVisibleItemIndex != 0;
        (BindingContext as ReferencesPageViewModel)!.HopReferenceViewModel.GoUpPageButtonVisible = topScrollViewScrolled || collectionViewScrolled; 
    }
    
    private async void GoUpButtonClicked(object? sender, EventArgs e)
    {
        // Go to origin of view
        var task = TopScrollView.ScrollToAsync(0, 0, true);
        HopCardsCollectionView.ScrollTo(0);
        await task;
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
}