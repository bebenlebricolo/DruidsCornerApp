namespace DruidsCornerApp.Views;
using DruidsCornerApp.Services.Framework;

/// <summary>
/// Abstract class (like a trait)  that'll be used to force HotReload on Code Behind as well
/// So that we can benefit of Hot reloading while developing the UI and not having to restart the whole app again
/// whenever we want to check if a UI change has made something !
/// </summary>
public abstract class AutoReloadPage : ContentPage
{

    /// <summary>
    /// Child page will need to implement this Build Method in order to initialize their UI
    /// </summary>
    public abstract void Build();

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        Build();

#if DEBUG
        HotReloadService.UpdateApplicationEvent += ReloadUI;
#endif
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

#if DEBUG
        HotReloadService.UpdateApplicationEvent -= ReloadUI;
#endif
    }

    private void ReloadUI(Type[]? obj)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Build();
        });
    }

}