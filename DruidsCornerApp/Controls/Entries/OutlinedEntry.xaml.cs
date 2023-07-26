namespace DruidsCornerApp.Controls.Entries;

public partial class OutlinedEntry : ContentView
{
    #region PublicProperties

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set
        {
            SetValue(TextProperty, value);
            OnPropertyChanged();
        }
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set 
        {
            SetValue(PlaceholderProperty, value);
            OnPropertyChanged();
        }
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set
        {
            SetValue(TextColorProperty, value);
            OnPropertyChanged();
        }
    }

    public Color PlaceholderColor
    {
        get => (Color)GetValue(PlaceholderColorProperty);
        set
        {
            SetValue(PlaceholderColorProperty, value);
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

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set
        {
            SetValue(BorderColorProperty, value);
            OnPropertyChanged();
        }
    }

    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set
        {
            SetValue(CornerRadiusProperty, value);
            OnPropertyChanged();
        }
    }

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set
        {
            SetValue(IconProperty, value);
            OnPropertyChanged();
        }
    }

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set
        {
            SetValue(IsPasswordProperty, value);
            OnPropertyChanged();
        }
    }

    #endregion PublicProperties


    #region StaticBinders

    public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text),
                                                                          typeof(string),
                                                                          typeof(OutlinedEntry),
                                                                          defaultBindingMode:BindingMode.TwoWay,
                                                                          propertyChanged: OnTextPropertyChanged
                                                                         );

    public static BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder),
                                                                                 typeof(string),
                                                                                 typeof(OutlinedEntry),
                                                                                 defaultBindingMode:BindingMode.TwoWay,
                                                                                 propertyChanged: OnPlaceholderPropertyChanged
                                                                                );

    public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
                                                                               typeof(Color),
                                                                               typeof(OutlinedEntry),
                                                                               defaultBindingMode:BindingMode.TwoWay,
                                                                               propertyChanged: OnTextColorPropertyChanged
                                                                              );


    public static BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor),
                                                                                      typeof(Color),
                                                                                      typeof(OutlinedEntry),
                                                                                      defaultBindingMode:BindingMode.TwoWay,
                                                                                      propertyChanged: OnPlaceholderColorPropertyChanged
                                                                                     );

    public new static BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor),
                                                                                         typeof(Color),
                                                                                         typeof(OutlinedEntry),
                                                                                         defaultBindingMode:BindingMode.TwoWay,
                                                                                         propertyChanged: OnBackgroundColorPropertyChanged
                                                                                        );

    public static BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor),
                                                                                 typeof(Color),
                                                                                 typeof(OutlinedEntry),
                                                                                 propertyChanged: OnBorderColorPropertyChanged
                                                                                );

    public static BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius),
                                                                                  typeof(double),
                                                                                  typeof(OutlinedEntry),
                                                                                  defaultBindingMode:BindingMode.TwoWay,
                                                                                  propertyChanged: OnCornerRadiusPropertyChanged
                                                                                 );

    public static BindableProperty IconProperty = BindableProperty.Create(nameof(Icon),
                                                                          typeof(string),
                                                                          typeof(OutlinedEntry),
                                                                          defaultBindingMode:BindingMode.TwoWay,
                                                                          propertyChanged: OnIconPropertyChanged
                                                                         );

    public static BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword),
                                                                                typeof(bool),
                                                                                typeof(OutlinedEntry),
                                                                                defaultBindingMode:BindingMode.TwoWay,
                                                                                propertyChanged: OnIsPasswordPropertyChanged
                                                                               );

    #endregion StaticBinders

    public OutlinedEntry()
    {
        InitializeComponent();

        // Register content-based events
        //EntryText.TextChanged += (sender, args) => { Text = args.NewTextValue; };
    }

    #region BindersEvents

    private static void OnTextPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedEntry)bindable;
        control.Text = (string)newvalue;
        control.EntryText.Text = control.Text;
        control.InvalidateLayout();
    }

    private static void OnPlaceholderPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedEntry)bindable;
        control.Placeholder = (string)newvalue;
        control.EntryText.Placeholder = control.Placeholder;
        control.InvalidateLayout();
    }

    private static void OnTextColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedEntry)bindable;
        control.TextColor = (Color)newvalue;
        control.EntryText.TextColor = control.TextColor;
        control.InvalidateLayout();
    }

    private static void OnPlaceholderColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedEntry)bindable;
        control.PlaceholderColor = (Color)newvalue;
        control.EntryText.PlaceholderColor = control.PlaceholderColor;
        control.InvalidateLayout();
    }

    private static void OnBackgroundColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedEntry)bindable;
        control.BackgroundColor = (Color)newvalue;
        control.EntryFrame.BackgroundColor = control.BackgroundColor;
        control.InvalidateLayout();
    }

    private static void OnBorderColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedEntry)bindable;
        control.BorderColor = (Color)newvalue;
        control.EntryFrame.BorderColor = control.BorderColor;
        control.InvalidateLayout();
    }

    private static void OnCornerRadiusPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedEntry)bindable;
        control.CornerRadius = (double)newvalue;
        control.EntryFrame.CornerRadius = Convert.ToSingle(control.CornerRadius);
        control.InvalidateLayout();
    }

    private static void OnIconPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedEntry)bindable;
        control.Icon = (string)newvalue;
        control.EntryIcon.Source = ImageSource.FromFile(control.Icon);
        control.InvalidateLayout();
    }

    private static void OnIsPasswordPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (OutlinedEntry)bindable;
        control.IsPassword = (bool)newvalue;
        control.EntryText.IsPassword = control.IsPassword;
        control.InvalidateLayout();
    }

    #endregion BindersEvents
}