using DruidsCornerApp.Controls.MainContext;
using DruidsCornerApp.Models.MainContext;
using DruidsCornerApp.ViewModels.MainContext;
using Microsoft.Extensions.Primitives;

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
        
        //HopCardsCollectionView.BindingContext = BindingContext;
        // HopCardsCollectionView.SetBinding(ItemsView.ItemsSourceProperty, "Hops");
        // HopCardsCollectionView.ItemTemplate = new DataTemplate(() =>
        // {
        //     var cardView = new HopCardView();
        //     cardView.BindingContext = BindingContext;
        //     return cardView;
        // });
    }
    
    private async void GoUpButtonClicked(object? sender, EventArgs e)
    {
        // Go to origin of view
        var task = TopScrollView.ScrollToAsync(0, 0, true);
        HopCardsCollectionView.ScrollTo(0);
        await task;
    }

    private void TopScrollView_OnScrolled(object? sender, ScrolledEventArgs e)
    {
        // Only trigger button visibility if we are scrolling the document
        GoUpPageButton.IsVisible = TopScrollView.ScrollY != 0;
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

    private void DataCollectionView_OnChildAdded(object? sender, ElementEventArgs e)
    {
        // Display the heart icon if hop is in favorite
        var frame = (Frame)e.Element;
        var hopModel = (CompactHopModel)frame.BindingContext;
        var heartImage = frame.FindByName<Image>("FavoriteHeartImage");
        if (heartImage != null)
        {
            if (hopModel.Favorite)
            {
                heartImage.Source = _heartFullSource;
            }
            else
            {
                heartImage.Source = _heartSource;
            }
            
        }
    }
}