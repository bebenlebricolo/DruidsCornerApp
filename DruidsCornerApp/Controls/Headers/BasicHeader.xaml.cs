using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Mvvm.Input;

namespace DruidsCornerApp.Controls.Headers;

public partial class BasicHeader : ContentView
{
    #region PublicProperties

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set
        {
            SetValue(TitleProperty, value);
            OnPropertyChanged();
        }
    }

    public Color TitleColor
    {
        get => (Color)GetValue(TitleColorProperty);
        set
        {
            SetValue(TitleColorProperty, value);
            OnPropertyChanged();
        }
    }
    
    public double TitleFontSize
    {
        get => (double)GetValue(TitleFontSizeProperty);
        set
        {
            SetValue(TitleFontSizeProperty, value);
            OnPropertyChanged();
        }
    }
    
    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set
        {
            SetValue(BackgroundColorProperty, value);
            OnPropertyChanged();
        }
    }
    
    public string BackIcon
    {
        get => (string)GetValue(BackIconProperty);
        set
        {
            SetValue(BackIconProperty, value);
            OnPropertyChanged();
        }
    }

    public double BackIconHeightRequest
    {
        get => (double)GetValue(BackIconHeightRequestProperty);
        set
        {
            SetValue(BackIconHeightRequestProperty, value);
            OnPropertyChanged();
        }
    }

    public double BackIconWidthRequest
    {
        get => (double)GetValue(BackIconWidthRequestProperty);
        set
        {
            SetValue(BackIconWidthRequestProperty, value);
            OnPropertyChanged();
        }
    }

    public Color BackIconColor
    {
        get => (Color)GetValue(BackIconColorProperty);
        set
        {
            SetValue(BackIconColorProperty, value);
            OnPropertyChanged();
        }
    }

    #endregion PublicProperties


    #region StaticBinders

    public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title),
                                                                          typeof(string),
                                                                          typeof(BasicHeader),
                                                                          defaultBindingMode: BindingMode.TwoWay,
                                                                          propertyChanged: OnTitlePropertyChanged
                                                                         );

    public static BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor),
                                                                               typeof(Color),
                                                                               typeof(BasicHeader),
                                                                               defaultBindingMode: BindingMode.TwoWay,
                                                                               propertyChanged: OnTitleColorPropertyChanged
                                                                              );

    public static BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize),
                                                                                typeof(double),
                                                                                typeof(BasicHeader),
                                                                                defaultBindingMode: BindingMode.TwoWay,
                                                                                propertyChanged: OnTitleFontSizePropertyChanged
                                                                               );


    public new static BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor),
                                                                                         typeof(Color),
                                                                                         typeof(BasicHeader),
                                                                                         defaultBindingMode: BindingMode.TwoWay,
                                                                                         propertyChanged: OnBackgroundColorPropertyChanged
                                                                                        );

  
    public static BindableProperty BackIconProperty = BindableProperty.Create(nameof(BackIcon),
                                                                          typeof(string),
                                                                          typeof(BasicHeader),
                                                                          defaultBindingMode: BindingMode.TwoWay,
                                                                          propertyChanged: OnBackIconPropertyChanged
                                                                         );

    public static BindableProperty BackIconHeightRequestProperty = BindableProperty.Create(nameof(BackIconHeightRequest),
                                                                                       typeof(double),
                                                                                       typeof(BasicHeader),
                                                                                       defaultBindingMode: BindingMode.TwoWay,
                                                                                       propertyChanged: OnBackIconHeightRequestPropertyChanged
                                                                                      );

    public static BindableProperty BackIconWidthRequestProperty = BindableProperty.Create(nameof(BackIconWidthRequest),
                                                                                       typeof(double),
                                                                                       typeof(BasicHeader),
                                                                                       defaultBindingMode: BindingMode.TwoWay,
                                                                                       propertyChanged: OnBackIconWidthRequestPropertyChanged
                                                                                      );

    public static BindableProperty BackIconColorProperty = BindableProperty.Create(nameof(BackIconColor),
                                                                               typeof(Color),
                                                                               typeof(BasicHeader),
                                                                               defaultBindingMode: BindingMode.TwoWay,
                                                                               propertyChanged: OnBackIconColorPropertyChanged
                                                                              );
    #endregion StaticBinders

    public BasicHeader()
    {
        InitializeComponent();
    }

    #region BindersEvents

    private static void OnTitlePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (BasicHeader) bindable;
        control.Title = (string) newvalue;
        control.TitleLabel.Text = control.Title;
        control.InvalidateLayout();
    }
    
    private static void OnTitleFontSizePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (BasicHeader) bindable;
        control.TitleFontSize = (double) newvalue;
        control.TitleLabel.FontSize = control.TitleFontSize; 
        control.InvalidateLayout();
    }

    private static void OnTitleColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (BasicHeader) bindable;
        control.TitleColor = (Color) newvalue;
        control.TitleLabel.TextColor = control.TitleColor;
        control.InvalidateLayout();
    }
    
    private static void OnBackgroundColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (BasicHeader)bindable;
        control.BackgroundColor = (Color)newvalue;
        control.OuterFrame.BackgroundColor = control.BackgroundColor;
        control.InvalidateLayout();
    }

    private static void OnBackIconPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (BasicHeader)bindable;
        control.BackIcon = (string)newvalue;
        control.BackButton.Source = ImageSource.FromFile(control.BackIcon);
        control.InvalidateLayout();
    }
    
    private static void OnBackIconHeightRequestPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (BasicHeader)bindable;
        control.BackIconHeightRequest = (double)newvalue;
        control.BackButton.HeightRequest = control.BackIconHeightRequest ;
        control.InvalidateLayout();
    }
    
    private static void OnBackIconWidthRequestPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (BasicHeader)bindable;
        control.BackIconWidthRequest = (double)newvalue;
        control.BackButton.WidthRequest = control.BackIconWidthRequest;
        control.InvalidateLayout();
    }
    
    private static void OnBackIconColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (BasicHeader)bindable;
        control.BackIconColor = (Color) newvalue;
        var behavior = (IconTintColorBehavior) control.BackButton.Behaviors[0];
        behavior.TintColor = control.BackIconColor;
        control.InvalidateLayout();
    }
    
    #endregion BindersEvents


    [RelayCommand]
    public async Task BackButtonClicked()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    private async void OnBackButtonClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..", true);
    }
}