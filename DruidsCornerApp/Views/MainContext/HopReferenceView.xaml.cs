namespace DruidsCornerApp.Views.MainContext;

/// <summary>
/// Hop reference page ; view model is linked to ReferencePageViewModel, as this component is "just" a view.
/// </summary>
public partial class HopReferenceView : ContentView
{
    public HopReferenceView()
    {
        InitializeComponent();
    }

    private async void GoUpButtonClicked(object? sender, EventArgs e)
    {
        // Go to origin of view
        var task = TopScrollView.ScrollToAsync(0, 0, true);
        DataCollectionView.ScrollTo(0);
        await task;
    }
}