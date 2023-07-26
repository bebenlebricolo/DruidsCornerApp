using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Handlers;

namespace DruidsCornerApp.Controls.Entries;

public partial class OutlinedPasswordEntry : OutlinedEntry
{
    private readonly ImageButton _button;
    private ImageSource _hiddenIcon;
    private ImageSource _shownIcon;

    #region PublicProperties

    // public IView HideShowIcon
    // {
    //     get => (IView)GetValue(HideShowIconProperty);
    //     set
    //     {
    //         SetValue(HideShowIconProperty, value);
    //         OnPropertyChanged();
    //     }
    // }

    public string HideIcon
    {
        get => (string)GetValue(HideIconProperty);
        set
        {
            SetValue(HideIconProperty, value);
            OnPropertyChanged();
        }
    }

    public string ShowIcon
    {
        get => (string)GetValue(ShowIconProperty);
        set
        {
            SetValue(ShowIconProperty, value);
            OnPropertyChanged();
        }
    }

    public double HideShowIconHeightRequest
    {
        get => (double)GetValue(HideShowIconHeightRequestProperty);
        set
        {
            SetValue(HideShowIconHeightRequestProperty, value);
            OnPropertyChanged();
        }
    }

    public double HideShowIconWidthRequest
    {
        get => (double)GetValue(HideShowIconWidthRequestProperty);
        set
        {
            SetValue(HideShowIconWidthRequestProperty, value);
            OnPropertyChanged();
        }
    }
    
    public Color HideShowIconColor
    {
        get => (Color) GetValue(HideShowIconColorProperty);
        set
        {
            SetValue(HideShowIconColorProperty, value);
            OnPropertyChanged();
        }
    }

    #endregion PublicProperties

    #region StaticBinders

    // public static BindableProperty HideShowIconProperty = BindableProperty.Create("HideShowIcon",
    //                                                                           typeof(IView),
    //                                                                           typeof(OutlinedPasswordEntry),
    //                                                                           defaultBindingMode: BindingMode.TwoWay,
    //                                                                           propertyChanged: OnHideIconPropertyChanged
    //                                                                          );

    public static BindableProperty HideIconProperty = BindableProperty.Create(nameof(HideIcon),
                                                                              typeof(string),
                                                                              typeof(OutlinedPasswordEntry),
                                                                              defaultBindingMode: BindingMode.TwoWay,
                                                                              propertyChanged: OnHideIconPropertyChanged
                                                                             );

    public static BindableProperty ShowIconProperty = BindableProperty.Create(nameof(ShowIconProperty),
                                                                              typeof(string),
                                                                              typeof(OutlinedPasswordEntry),
                                                                              defaultBindingMode: BindingMode.TwoWay,
                                                                              propertyChanged: OnShowIconPropertyChanged
                                                                             );

    public static BindableProperty HideShowIconHeightRequestProperty = BindableProperty.Create(nameof(HideShowIconHeightRequest),
                                                                                               typeof(double),
                                                                                               typeof(OutlinedPasswordEntry),
                                                                                               defaultBindingMode: BindingMode.TwoWay,
                                                                                               propertyChanged:
                                                                                               OnHideShowIconHeightRequestPropertyChanged
                                                                                              );

    public static BindableProperty HideShowIconWidthRequestProperty = BindableProperty.Create(nameof(HideShowIconWidthRequest),
                                                                                              typeof(double),
                                                                                              typeof(OutlinedPasswordEntry),
                                                                                              defaultBindingMode: BindingMode.TwoWay,
                                                                                              propertyChanged:
                                                                                              OnHideShowIconWidthRequestPropertyChanged
                                                                                             );
    
    public static BindableProperty HideShowIconColorProperty = BindableProperty.Create(nameof(HideShowIconColor),
                                                                                              typeof(Color),
                                                                                              typeof(OutlinedPasswordEntry),
                                                                                              defaultBindingMode: BindingMode.TwoWay,
                                                                                              propertyChanged:
                                                                                              OnHideShowIconColorPropertyChanged
                                                                                             );

    #endregion StaticBinders

    public OutlinedPasswordEntry()
    {
        _button = new ImageButton();
        _button.Clicked += OnPasswordVisibiltyTriggered;
        _button.HorizontalOptions = LayoutOptions.End;
        _button.VerticalOptions = LayoutOptions.Center;
        _button.Aspect = Aspect.AspectFit;
        _button.HeightRequest = EntryIcon.HeightRequest;
        _button.Behaviors.Add(new IconTintColorBehavior()
        {
            TintColor = Colors.Grey 
        });
        
        // By default, this is a password entry after all!
        IsPassword = true;
        
        try
        {
            _hiddenIcon = ImageSource.FromFile(HideIcon);
            _shownIcon = ImageSource.FromFile(HideIcon);
        }
        catch (Exception)
        {
            // Do nothing
        }

        // Configure password visibility with icon state
        HandleIconStates();

        // Add a new column to fit in the button
        GridContent.ColumnDefinitions.Add( new ColumnDefinition(new GridLength(30)));
        GridContent.Add(_button, 3, 0);
    }

    protected void HandleIconStates()
    {
        if (IsPassword)
        {
            _button.Source = _hiddenIcon;
        }
        else
        {
            _button.Source = _shownIcon;
        }
    }

    private void OnPasswordVisibiltyTriggered(object? sender, EventArgs e)
    {
        IsPassword = !IsPassword;
        HandleIconStates();
    }

    #region BindersEvents

    private static void OnHideIconPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedPasswordEntry)bindable;
        control.HideIcon = (string)newvalue;
        control._hiddenIcon = ImageSource.FromFile(control.HideIcon);
        control.HandleIconStates();
        control.InvalidateLayout();
    }

    private static void OnShowIconPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedPasswordEntry)bindable;
        control.ShowIcon = (string)newvalue;
        control._shownIcon = ImageSource.FromFile(control.ShowIcon);
        control.HandleIconStates();
        control.InvalidateLayout();
    }

    private static void OnHideShowIconHeightRequestPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedPasswordEntry)bindable;
        control.HideShowIconHeightRequest = (double) newvalue;
        control._button.HeightRequest = control.HideShowIconHeightRequest;
        control.InvalidateLayout();
    }

    private static void OnHideShowIconWidthRequestPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedPasswordEntry)bindable;
        control.HideShowIconWidthRequest = (double) newvalue;
        control._button.WidthRequest = control.HideShowIconWidthRequest;
        control.InvalidateLayout();
    }
    
    
    private static void OnHideShowIconColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedPasswordEntry)bindable;
        control.HideShowIconColor = (Color) newvalue;
        var behavior = (IconTintColorBehavior)control._button.Behaviors[0];
        behavior.TintColor = control.HideShowIconColor;
        control.InvalidateLayout();
    }
        
        
    #endregion BindersEvents
}