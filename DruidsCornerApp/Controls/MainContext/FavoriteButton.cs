using System.Reflection.Metadata;
using DruidsCornerApp.Services.Config;
using DruidsCornerApp.Utils;

namespace DruidsCornerApp.Controls.MainContext;

public partial class FavoriteButton : ContentView
{
    private ImageSource? _defaultIconSource = null;
    private ImageSource? _favoriteIconSource = null;

    private readonly Image _icon;
    
    /// <summary>
    /// Used to switch between the two image sources.
    /// </summary>
    public bool Favorite
    {
        get => (bool)GetValue(FavoriteProperty);
        set
        {
            SetValue(FavoriteProperty, value);
            OnPropertyChanged();
        }
    }
    
    /// <summary>
    /// Favorite icon, when the item is set as "favorite"
    /// </summary>
    public string FavoriteIcon
    {
        get => (string)GetValue(FavoriteIconProperty);
        set
        {
            SetValue(FavoriteIconProperty, value);
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Default icon, when the item is not set as "favorite"
    /// </summary>
    public string DefaultIcon
    {
        get => (string) GetValue(DefaultIconProperty);
        set
        {
            SetValue(DefaultIconProperty, value);
            OnPropertyChanged();
        }
    }
    
    /// <summary>
    /// Gives the requested icon size (both in x and y
    /// </summary>
    public double RequestedIconSize
    {
        get => (double) GetValue(RequestedIconSizeProperty);
        set
        {
            SetValue(RequestedIconSizeProperty, value);
            OnPropertyChanged();
        }
    }

    public static BindableProperty FavoriteProperty = BindableProperty.Create(nameof(Favorite),
                                                                              typeof(bool),
                                                                              typeof(FavoriteButton),
                                                                              defaultBindingMode: BindingMode.TwoWay,
                                                                              propertyChanged: OnFavoritePropertyChanged);

    public static BindableProperty FavoriteIconProperty = BindableProperty.Create(nameof(FavoriteIcon),
                                                                                  typeof(string),
                                                                                  typeof(FavoriteButton),
                                                                                  defaultBindingMode: BindingMode.TwoWay,
                                                                                  propertyChanged: OnFavoriteIconPropertyChanged);

    public static BindableProperty DefaultIconProperty = BindableProperty.Create(nameof(DefaultIconProperty),
                                                                                 typeof(string),
                                                                                 typeof(FavoriteButton),
                                                                                 defaultBindingMode: BindingMode.TwoWay,
                                                                                 propertyChanged: OnDefaultIconPropertyChanged);

    public static BindableProperty RequestedIconSizeProperty = BindableProperty.Create(nameof(RequestedIconSize),
                                                                                       typeof(double),
                                                                                       typeof(FavoriteButton),
                                                                                       defaultBindingMode: BindingMode.TwoWay,
                                                                                       propertyChanged:
                                                                                       OnRequestedIconSizePropertyChanged);
    
    private static void OnFavoritePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (FavoriteButton) bindable;
        control.Favorite = (bool) newvalue;
        control.HandleIconStates();
    }

    private static void OnFavoriteIconPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (FavoriteButton) bindable;
        control.FavoriteIcon = (string) newvalue;
        control._favoriteIconSource = IconsProvider.Instance().GetFile(control.FavoriteIcon);
        control.HandleIconStates();
    }

    private static void OnDefaultIconPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (FavoriteButton) bindable;
        control.DefaultIcon = (string) newvalue;
        control._defaultIconSource = IconsProvider.Instance().GetFile(control.DefaultIcon);
        control.HandleIconStates();
    }

    private static void OnRequestedIconSizePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (FavoriteButton) bindable;
        control.RequestedIconSize = (double) newvalue;
        control._icon.HeightRequest = control.RequestedIconSize;
        control._icon.WidthRequest = control.RequestedIconSize;
    }


    public FavoriteButton()
    {
        _icon = new Image();
        _icon.Source = IconsProvider.Instance().GetFile("heart_full.svg");
        Content = _icon;
        
        // Add the "Tapped" button event mapping
        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnFavoriteIconToggled;
        GestureRecognizers.Add(tapGestureRecognizer);
    }

    private void HandleIconStates()
    {
        if (Favorite)
        {
            _icon.Source = _favoriteIconSource;
        }
        else
        {
            _icon.Source = _defaultIconSource;
        }
    }

    private void OnFavoriteIconToggled(object? sender, TappedEventArgs e)
    {
        Favorite = !Favorite;
    }

    /// <summary>
    /// Triggered by the parent page, has to be in the code behind (...)
    /// </summary>
    public void OnPageAppearing()
    {
        HandleIconStates();
    }
}